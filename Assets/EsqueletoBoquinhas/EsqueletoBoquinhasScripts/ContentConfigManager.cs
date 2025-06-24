using UnityEngine;

public class ContentConfigManager : MonoBehaviour
{
    public GameObject NarrationYesObject;
    public GameObject NarrationNoObject;
    public GameObject MusicSfxYesObject;
    public GameObject MusicSfxObject;
    public GameObject TimeYesObject;
    public GameObject TimeNoObject;
    public AudioClip narrationYes;
    public AudioClip narrationNo;
    public AudioClip musicSfxYes;
    public AudioClip musicSfxNo;
    public AudioClip timeYes;
    public AudioClip timeNo;

    private bool isMusicSfxOn = true;
    private bool isNarrationOn = true;
    private bool isTimeOn = true;

    private void Start()
    {
        Load();
        RefreshUI();
    }

    #region UI

    private void RefreshUI()
    {
        // Narration
        if (isNarrationOn)
        {
            NarrationYesObject.SetActive(true);
            NarrationNoObject.SetActive(false);
        }
        else
        {
            NarrationYesObject.SetActive(false);
            NarrationNoObject.SetActive(true);
        }

        // SFX
        if (isMusicSfxOn)
        {
            MusicSfxYesObject.SetActive(true);
            MusicSfxObject.SetActive(false);
        }
        else
        {
            MusicSfxYesObject.SetActive(false);
            MusicSfxObject.SetActive(true);
        }

        // Time
        if (isTimeOn)
        {
            TimeYesObject.SetActive(true);
            TimeNoObject.SetActive(false);
        }
        else
        {
            TimeYesObject.SetActive(false);
            TimeNoObject.SetActive(true);
        }
    }

    #endregion

    #region Toggles

    public void ToggleMusicSfx()
    {
        if (isMusicSfxOn)
        {
            isMusicSfxOn = false;
            FindObjectOfType<SettingsSaveAndLoad>().musicSfxOn = false;
            AudioSystem.Instance.MuteMusic();
            AudioSystem.Instance.MuteSfx();
            AudioSystem.Instance.PlayUnSilenceableNarration(musicSfxNo);
        }
        else
        {
            isMusicSfxOn = true;
            FindObjectOfType<SettingsSaveAndLoad>().musicSfxOn = true;
            FindObjectOfType<AutoMusicManager>().RecheckMusicAfterLoad();
            AudioSystem.Instance.UnmuteMusic();
            AudioSystem.Instance.UnmuteSfx();
            AudioSystem.Instance.PlayUnSilenceableNarration(musicSfxYes);
        }

        Save();
        RefreshUI();
    }

    public void ToggleNarration()
    {
        if (isNarrationOn)
        {
            isNarrationOn = false;
            AudioSystem.Instance.MuteNarration();
            AudioSystem.Instance.PlayUnSilenceableNarration(narrationNo);
        }
        else
        {
            isNarrationOn = true;
            AudioSystem.Instance.UnmuteNarration();
            AudioSystem.Instance.PlayUnSilenceableNarration(narrationYes);
        }

        Save();
        RefreshUI();
    }

    public void ToggleTime()
    {
        if (isTimeOn)
        {
            isTimeOn = false;
            TimerSystem.Instance.isTimerEnabled = false;
            AudioSystem.Instance.PlayUnSilenceableNarration(timeNo);
        }
        else
        {
            isTimeOn = true;
            TimerSystem.Instance.isTimerEnabled = true;
            AudioSystem.Instance.PlayUnSilenceableNarration(timeYes);
        }

        Save();
        RefreshUI();
    }

    #endregion

    #region Save/Load

    private void Save()
    {
        PlayerPrefs.SetInt("isMusicSfxOn", isMusicSfxOn ? 1 : 0);
        PlayerPrefs.SetInt("isNarrationOn", isNarrationOn ? 1 : 0);
        PlayerPrefs.SetInt("isTimeOn", isTimeOn ? 1 : 0);
    }

    private void Load()
    {
        isMusicSfxOn = PlayerPrefs.GetInt("isMusicSfxOn", 1) == 1;
        isNarrationOn = PlayerPrefs.GetInt("isNarrationOn", 1) == 1;
        isTimeOn = PlayerPrefs.GetInt("isTimeOn", 1) == 1;

        if (isMusicSfxOn)
        {
            AudioSystem.Instance.musicVolume = AudioSystem.Instance.originalMusicVolume;
            AudioSystem.Instance.silenceableSfxVolume = 1;
        }
        else
        {
            AudioSystem.Instance.musicVolume = 0;
            AudioSystem.Instance.silenceableSfxVolume = 0;
        }

        if (isNarrationOn)
            AudioSystem.Instance.silenceableNarrationVolume = 1;
        else
            AudioSystem.Instance.silenceableNarrationVolume = 0;

        if (isTimeOn)
            TimerSystem.Instance.isTimerEnabled = true;
        else
            TimerSystem.Instance.isTimerEnabled = false;
    }

    #endregion
}