using Boquinhas.ConfigurationSettings;
using Boquinhas.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public static TimeBar instance;

    public Image filledBarImage;
    public TextMeshProUGUI timeText;

    public Sprite roundOn;
    public Image[] bigRoundDots;
    public Image[] smallRoundDots;
    public AudioClip TimeWarningClip;
    public AudioClip timeOver;
    public float timeWarning = 10f;
    public bool timerOn;

    private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;

    private float initialTime = 10f;

    private readonly bool noNarration = true;
    private AudioClip timeAlmostOver;

    private bool timeAlmostOverPlayed;
    private float timer = 10f;
    private bool timeWarningPlayed;
    private RoundManager _roundManager;
    private bool infiniteRound = false;
    
    private void Awake()
    {
        _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
        _roundManager = FindObjectOfType<RoundManager>();
    }

    private void Start()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;
        
        if (_boquinhasConfigurationSettings.GetMaxRounds() == 0)
        {
            //Infinite rounds
            if (GlobalDebugs.Instance.DebugTimer) Debug.Log("Debug Timer: Infinite Rounds");
            
            DeactivateAllDots();
            infiniteRound = true;
        }else if (_boquinhasConfigurationSettings.GetMaxRounds() <= 6)
        {
            //6 or less rounds (Big dots)
            if (GlobalDebugs.Instance.DebugTimer) Debug.Log("Debug Timer: More than 6 rounds");
            
            ActivateBigRoundDots(_boquinhasConfigurationSettings.GetMaxRounds());
        }
        else
        {
            //7 or more rounds (Small dots)
            if (GlobalDebugs.Instance.DebugTimer) Debug.Log("Debug Timer: Rounds: " + _boquinhasConfigurationSettings.GetMaxRounds());
            
            int rounds = _boquinhasConfigurationSettings.GetMaxRounds();
            
            if (rounds > smallRoundDots.Length)
            {
                Debug.LogError("Amount of rounds in the game exceeds the amount of small dots available on screen, game WILL CRASH!");
                return;
            }
            else
            {
                ActivateSmallRoundDots(_boquinhasConfigurationSettings.GetMaxRounds());
            }
        }
    }

    private void FixedUpdate()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;

        if (timerOn)
        {
            timer -= Time.fixedDeltaTime;
            if (timer >= 0f && timer <= initialTime)
            {
                var t = (int)(timer + 0.9f);
                timeText.text = t.ToString();
            }

            filledBarImage.fillAmount = timer / initialTime;
            if (filledBarImage.fillAmount > 0.667f)
            {
                filledBarImage.color = Color.green;
            }
            else if (filledBarImage.fillAmount <= 0.667f && filledBarImage.fillAmount > 0.334f)
            {
                filledBarImage.color = Color.yellow;
            }
            else
            {
                filledBarImage.color = Color.red;
                if (!timeAlmostOverPlayed && !noNarration)
                {
                    timeAlmostOverPlayed = true;
                    //XGH
                    if (FindObjectOfType<ForceSingleAudio>() != null)
                    {
                        if (!FindObjectOfType<ForceSingleAudio>().AnotherAudioIsPlaying())
                            AudioSystem.Instance.PlayUnSilenceableNarration(timeAlmostOver);
                    }
                    else
                    {
                        AudioSystem.Instance.PlayUnSilenceableNarration(timeAlmostOver);
                    }
                }
            }

            if (timer <= timeWarning && !timeWarningPlayed)
            {
                timeWarningPlayed = true;
                AudioSystem.Instance.PlaySilenceableSfx(TimeWarningClip);
            }

            if (timer <= 0f)
            {
                AudioSystem.Instance.PlaySilenceableSfx(timeOver);
                timerOn = false;
                _roundManager.EndTimer();
            }
        }
    }

    private void OnEnable()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;
        switch (GameManager.Instance.GetCurrentDifficultyLevel())
        {
            case DifficultyLevels.Easy:
                initialTime = _boquinhasConfigurationSettings.gameDurationEasy;
                break;
            case DifficultyLevels.Medium:
                initialTime = _boquinhasConfigurationSettings.gameDurationMedium;
                break;
            case DifficultyLevels.Hard:
                initialTime = _boquinhasConfigurationSettings.gameDurationHard;
                break;
            case DifficultyLevels.Letters:
                initialTime = _boquinhasConfigurationSettings.gameDurationEasy;
                break;
            case DifficultyLevels.Mouths:
                initialTime = _boquinhasConfigurationSettings.gameDurationEasy;
                break;
            case DifficultyLevels.Figures:
                initialTime = _boquinhasConfigurationSettings.gameDurationEasy;
                break;
        }

        instance = this;
        RestartTimer();
    }

    private void OnDisable()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;

        if (instance == this)
        {
            instance = null;
        }
       
    }

    public void StartTimer()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;

        timerOn = true;
    }

    public void StopTimer()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;

        timerOn = false;
    }

    public void UpdateRounds(int round)
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;
        if (infiniteRound) return;
        
        if (_boquinhasConfigurationSettings.GetMaxRounds() > 6)
        {
            //Small dots
            smallRoundDots[round - 1].sprite = roundOn;
        }
        else
        {
            //Big dots
            bigRoundDots[round - 1].sprite = roundOn;
        }
    }

    public void RestartTimer()
    {
        if (!TimerSystem.Instance.isTimerEnabled) return;

        filledBarImage.fillAmount = 1f;
        filledBarImage.color = Color.green;
        timer = initialTime;
        timeText.text = initialTime.ToString();
        timeAlmostOverPlayed = false;
    }
    
    private void ActivateBigRoundDots(int amount)
    {
        DeactivateAllDots();
        
        for (var i = 0; i < bigRoundDots.Length; i++)
        {
            bigRoundDots[i].gameObject.SetActive(i < amount);
        }
    }
    
    private void ActivateSmallRoundDots(int amount)
    {
        DeactivateAllDots();
        
        for (var i = 0; i < smallRoundDots.Length; i++)
        {
            smallRoundDots[i].gameObject.SetActive(i < amount);
        }
    }

    private void DeactivateAllDots()
    {
        foreach (var dot in bigRoundDots)
        {
            dot.gameObject.SetActive(false);
        }
        
        foreach (var dot in smallRoundDots)
        {
            dot.gameObject.SetActive(false);
        }
    }
}