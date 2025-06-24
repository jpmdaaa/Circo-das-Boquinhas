using Boquinhas.ConfigurationSettings;
using Boquinhas.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundAssetController : MonoBehaviour
{
    public Image roundNumber;
    public Image oldRoundNumber;
    public TextMeshProUGUI maxRound;

    public Sprite[] roundNumberSprites;
    public AudioClip[] roundAudios;
    public AudioClip newRoundSfx;
    private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;

    private Animator m_Animator;

    private void Awake()
    {
        _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        maxRound.text = "/ " + _boquinhasConfigurationSettings.GetMaxRounds();
    }

    public void ChangeRoundNumber(int i)
    {
        AudioSystem.Instance.PlaySilenceableSfx(newRoundSfx);
        
        if (i <= roundNumberSprites.Length)
        {
            AudioSystem.Instance.PlaySilenceableNarration(roundAudios[i - 1]);
            roundNumber.sprite = roundNumberSprites[i - 1];   
        }else
        {
            Debug.LogError("Round number " + i + " is not implemented yet, we are missing the sprite and audio for this round");
        }
    }

    public void ChangeOldRoundNumber()
    {
        oldRoundNumber.sprite = roundNumber.sprite;
    }

    public void SetAnimatorTrigger(string trigger)
    {
        m_Animator.SetTrigger(trigger);
    }
}