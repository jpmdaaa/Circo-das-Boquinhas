using System;
using Boquinhas.Core;
using UnityEngine;

public class ExampleGameplayWithRoundManager : MonoBehaviour
{
    public RoundManager roundManager;

    private int errorCount = 0;

    private void Start()
    {
        Debug.Log("Current difficulty: " + GameManager.Instance.GetCurrentDifficultyLevel());
    }

    public void StartGameAfterTutorial()
    {
        roundManager.StartRound();
    }

    public void StartTimer()
    {
        roundManager.StartTimer();
    }

    public void RightAnswer()
    {
        roundManager.CallAnswer(true, false);
    }

    public void WrongAnswer(bool pCanRetry = true)
    {
        errorCount++;
        if (errorCount >= 2)
        {
            roundManager.CallAnswer(false, true);
            errorCount = 0;
        }
        else
        {
            roundManager.CallAnswer(false, false);
        }
        
    }
    
    public void HalfRightAnswer(bool pCanRetry = true)
    {
        errorCount++;
        if (errorCount >= 2)
        {
            roundManager.CallAnswer(false, true);
            errorCount = 0;
        }
        else
        {
            roundManager.CallAnswer(false, false);
        }
    }
    
    public void TimeEnded()
    {
        Debug.Log("Time Ended");
        //roundManager.CallAnswer(false);
    }
    
    
    
    //LOGS
    
    public void LogContinueAfterSecondWrongAnswer()
    {
        Debug.Log("Continue after second wrong answer");
    }
    
    public void LogRoundStart()
    {
        Debug.Log("Round Start");
    }
    
    public void LogRoundEnd()
    {
        Debug.Log("Round End");
    }
}