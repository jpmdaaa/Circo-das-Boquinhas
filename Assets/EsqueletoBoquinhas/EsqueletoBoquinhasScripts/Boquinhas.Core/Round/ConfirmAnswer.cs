using Boquinhas.ConfigurationSettings;
using Boquinhas.Core;
using UnityEngine;

public class ConfirmAnswer : MonoBehaviour
{
    public RoundManager roundManager;
    public AudioClip rightAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioClip wrongAnswerNarration;
    public AudioClip secondWrongAnswerNarration;
    public AudioClip halfRightNarration;
    public AudioClip[] rightAnswerNarrations;
    private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;
    private Animator m_Animator;


    private void Awake()
    {
        _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
        m_Animator = GetComponent<Animator>();
    }

    private void CallAnimatorTrigger(string trigger)
    {
        m_Animator.SetTrigger(trigger);
    }

    public void CallRightAnswer()
    {
        if (TimerSystem.Instance.isTimerEnabled) TimeBar.instance.StopTimer();
        CallAnimatorTrigger("right");
        AudioSystem.Instance.PlaySilenceableSfx(rightAnswerSound);
        AudioSystem.Instance.PlaySilenceableNarration(rightAnswerNarrations[Random.Range(0, rightAnswerNarrations.Length)]);
    }
    
    public void CallWrongAnswer()
    {
        if (TimerSystem.Instance.isTimerEnabled) TimeBar.instance.StopTimer();
        CallAnimatorTrigger("wrong");
        AudioSystem.Instance.PlaySilenceableSfx(wrongAnswerSound);
        AudioSystem.Instance.PlaySilenceableNarration(wrongAnswerNarration);
    }
    
    public void CallSecondWrongAnswer()
    {
        if (TimerSystem.Instance.isTimerEnabled) TimeBar.instance.StopTimer();
        CallAnimatorTrigger("second wrong");
        AudioSystem.Instance.PlaySilenceableSfx(wrongAnswerSound);
        AudioSystem.Instance.PlaySilenceableNarration(secondWrongAnswerNarration);
    }
    
    public void CallHalfRightAnswer()
    {
        if (TimerSystem.Instance.isTimerEnabled) TimeBar.instance.StopTimer();
        CallAnimatorTrigger("half right");
        AudioSystem.Instance.PlaySilenceableSfx(wrongAnswerSound);
        AudioSystem.Instance.PlaySilenceableNarration(halfRightNarration);
    }
    
    public void CallTimeEnded()
    {
        if (TimerSystem.Instance.isTimerEnabled) TimeBar.instance.StopTimer();
        CallAnimatorTrigger("wrong");
        AudioSystem.Instance.PlaySilenceableSfx(wrongAnswerSound);
    }

    public void NextPlayerButton()
    {
        //GameController.instance.PlayerGuessedRight();
        roundManager.PlayerGuessedRight();
        CallAnimatorTrigger("leave");
    }

    public void TryAgainButton()
    {
        //GameController.instance.PlayerGuessedWrong();
        roundManager.PlayerGuessedWrong();
        CallAnimatorTrigger("leave");
    }

    public void NextPlayerWrongButton()
    {
        roundManager.PlayerGuessedWrong(false);
        CallAnimatorTrigger("leave");
    }
    
    public void NextPlayerAfterSecondWrongButton()
    {
        roundManager.PlayerGuessedWrongSecondTime();
        CallAnimatorTrigger("leave");
    }
}