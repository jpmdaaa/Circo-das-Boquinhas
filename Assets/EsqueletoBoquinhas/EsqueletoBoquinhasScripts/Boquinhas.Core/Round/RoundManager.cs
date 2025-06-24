using System;
using System.Collections;
using System.Collections.Generic;
using Boquinhas.ConfigurationSettings;
using Boquinhas.Core;
using Playmove.Avatars.API;
using Playmove.Metrics.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class RoundManager : MonoBehaviour
{
    [Header("Events")] public UnityEvent GameStartAfertTutorialEvent;

    public UnityEvent GameplayStartEvent;
    public UnityEvent StartTimerEvent;
    public UnityEvent TimerEndEvent;
    public UnityEvent rightAnswerEvent;
    public UnityEvent wrongAnswerEvent;
    public UnityEvent twoWrongAnswersEvent;
    public UnityEvent continueAfterRightAnswerEvent;
    public UnityEvent ContinuerAferWrongAnswerEvent;
    public UnityEvent ContinueAfterSecondWrongAnswer;
    public UnityEvent TryAgainEvent;
    public UnityEvent RoundStartEvent;
    public UnityEvent RoundEndEvent;
    public UnityEvent GameEndEvent;
    public GameManagerBoquinhas gameManager;


    [Header(@"\/DONT TOUCH\/")] public RoundAssetController roundAssets;
    private bool autoMetricsExperimental = false;
    public ConfirmAnswer confirmAnswerPanel;
    public List<PlayerGameplay> players;
    public Transform guessParent;

    public GameObject Timer;

    public GameObject btnTryAgain;
    public GameObject btnContinue;

    public GameObject backCanvas;
    public GameObject blockerRoxo;
    
    private int _activePlayerNumber; //DONT USE THIS FOR METRICS
    public int activePlayerForMetrics; //USE THIS!
    
    private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;
    private List<int> _slotsPlaying;

    private int _activePlayerSlot; //DONT USE THIS FOR METRICS, USE activePlayerForMatrics!

    //public GameObject playerPanel;
    //public GameObject playerGuessPanel;
    private int player0errors;
    private int player1errors;
    private int player2errors;
    private int player3errors;
    private bool rightAnswerAchieved;


    private int roundNumber;
    public AudioClip roundEndNarration;
    public AudioClip turnEndNarration;
    public AudioClip gameEndNarration;


    [Header("Feedbacks")]
    public List<Sprite> spritesfeed; // lista Sprite de Imagens
    public List<Sprite> textosFeed;   // lista Sprite de textos
    public GameObject feedImage; // GO
    public GameObject feedtext;   // GO
    public GameObject goFeed;


    private void Awake()
    {
        goFeed.SetActive(false); 
        gameManager = (GameManagerBoquinhas)GameObject.FindObjectOfType(typeof(GameManagerBoquinhas));

        if (GlobalDebugs.Instance.AutoMetrics)
        {
            autoMetricsExperimental = true;
            Debug.Log("Auto Metrics is ON!");
        }
        else
        {
            autoMetricsExperimental = false;
        }
        
        _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
        if (_boquinhasConfigurationSettings.canTryAgain)
        {
            btnTryAgain.SetActive(true);
            btnContinue.SetActive(false);
        }
        else
        {
            btnContinue.SetActive(true);
            btnTryAgain.SetActive(false);
        }

        _slotsPlaying = new List<int>();
        foreach (var slot in AvatarAPI.CurrentSlots)
            if (slot.Players.Count > 0)
                _slotsPlaying.Add(slot.Pos);
    }

    private void Start()
    {
        if (autoMetricsExperimental) MetricsAPI.StartMatch(" ", " ", 0);
    }

    public void StartRound()
    {
        StartCoroutine(RoundStartCoroutine());
        blockerRoxo.SetActive(true);
    }

    private IEnumerator RoundStartCoroutine()
    {
        if (roundNumber > 0)
        {
            //EVENTO DE ROUND END
            RoundEndEvent.Invoke();
        }
        
        roundNumber++;
    
        if (_boquinhasConfigurationSettings.GetMaxRounds() > 0)
            if (roundNumber > _boquinhasConfigurationSettings.GetMaxRounds()) // T�rmino de jogo, finalizar partida
            {
                AudioSystem.Instance.PlaySilenceableNarration(gameEndNarration);
                if (autoMetricsExperimental) MetricsAPI.EndMatch(EndReasons.Victory, null);
                SpawnErrorHolder();
                //EVENTO DE GAME END
                GameEndEvent.Invoke();
                FindObjectOfType<ScenesSystem>().ChangeScene("GameEnd");
                yield break;
            }

        
        TurnPanel();
        RoundStartEvent.Invoke();

        if (_slotsPlaying[0] == 2 || _slotsPlaying[0] == 3)
        {
            //Só jogadores em cima, girar as instruções
            roundAssets.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            if (backCanvas != null) backCanvas.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            //backCanvas.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -180f));
        }
        else
        {
            roundAssets.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            if (backCanvas != null) backCanvas.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            //backCanvas.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        if (_boquinhasConfigurationSettings.GetMaxRounds() > 0)
        {
            roundAssets.gameObject.SetActive(true); // Anima��o de entrada da rodada come�a aqui        


            roundAssets.ChangeRoundNumber(roundNumber);
            yield return new WaitForSecondsRealtime(1f);
            if (roundNumber > 1) // Anima��o da rodada para troca de round quando n�o for o primeiro
            {
                roundAssets.SetAnimatorTrigger("changeRound");
                yield return new WaitForSecondsRealtime(1.5f);
            }
            else
            {
                roundAssets.SetAnimatorTrigger("firstRoundLeave");
                yield return new WaitForSecondsRealtime(1f);
            }

            yield return new WaitForSecondsRealtime(0.2f);
            roundAssets.gameObject.SetActive(false);
        }

        StartCoroutine(TurnStartCoroutine());
        yield return null;
    }

    private IEnumerator TurnStartCoroutine()
    {
        //print(GetCurrentPlayer());

        if (!players[_activePlayerNumber].gameObject
                .activeSelf) // Se o jogador n�o estiver setado e ativo desde o come�o, vai para o pr�ximo jogador
        {
            PassTurn();
            yield break;
        }

        _activePlayerSlot = players[_activePlayerNumber].slotNumber;
        
        //Crappy fix because they inverted the active player slots on top and dont want to change it back
        //Debug.Log("Active player from metricas before handling: " + _activePlayerSlot);
        if (_activePlayerSlot == 2)
        {
            //Debug.Log("Detected player in slot 2, fixing active player slot to 3");
            _activePlayerSlot = 3;
            activePlayerForMetrics = 3;
        }else if (_activePlayerSlot == 3)
        {
            //Debug.Log("Detected player in slot 3, fixing active player slot to 2");
            _activePlayerSlot = 2;
            activePlayerForMetrics = 2;
        }else{
            //Debug.Log("Active player slot is " + _activePlayerSlot + ", no fix needed");
            activePlayerForMetrics = _activePlayerSlot;
        }
        //Debug.Log("Active player for metrics after handling: " + _activePlayerSlot);
        
        
        //DEBUG-DEBUG-DEBUG-DEBUG-DEBUG-DEBUG-DEBUG-DEBUG-DEBUG-DEBUG
        if (autoMetricsExperimental) MetricsAPI.StartEvent(AvatarAPI.PlayersIdsFromSlot(activePlayerForMetrics), "Jogador iniciou um turno.");
        //EVENTO DE INICO DE TURNO
        GameStartAfertTutorialEvent.Invoke();
        TurnPanel();

        blockerRoxo.SetActive(true);
        //playerGuessPanel.SetActive(true);
        if (TimerSystem.Instance.isTimerEnabled)
        {
            Timer.SetActive(true);
            TimeBar.instance.UpdateRounds(roundNumber);
        }
        else
        {
            Timer.SetActive(false);
        }

        //players[activePlayerNumber].transform.SetParent(playerGuessPanel.transform);
        players[_activePlayerNumber].transform.SetSiblingIndex(0);

        players[_activePlayerNumber].EnterTurn();
        yield return new WaitForSecondsRealtime(1f); // Aguardar anima��o de entrada para o turno

        blockerRoxo.SetActive(false);
        players[_activePlayerNumber].EnterGuess();

        yield return new WaitForSecondsRealtime(1f); // Aguardar anima��o de sa�da do avatar para mostrar a carta

        //EVENTO DE GAMEPLAY LIBERADA
        GameplayStartEvent.Invoke();

        if (backCanvas != null) backCanvas.SetActive(true);

        //TimeBar.instance.StartTimer();

        // -- Loop retornar� aqui para tentativas incorretas --

        yield return new WaitUntil(() => rightAnswerAchieved); // Aguardar at� que uma resposta correta tenha sido dada

        yield return new WaitForSecondsRealtime(1f); // Anima��o de feedback positivo da carta devido � acerto

        if (backCanvas != null) backCanvas.SetActive(false);

        if (TimerSystem.Instance.isTimerEnabled) Timer.SetActive(false);
        //playerGuessPanel.SetActive(false);
        rightAnswerAchieved = false;

        PassTurn();
    }

    private void TurnPanel()
    {
        if (IsScreenRotated())
        {
            guessParent.eulerAngles = new Vector3(0f, 0f, 180f);
            if (backCanvas != null) backCanvas.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        else
        {
            guessParent.eulerAngles = Vector3.zero;
            if (backCanvas != null) backCanvas.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    public bool IsScreenRotated()
    {
        if (_slotsPlaying == null)
            return false;

        if (_slotsPlaying[0] == 2 || _slotsPlaying[0] == 3) // somente jogadores em cima
            return true;

        if (players[_activePlayerNumber].slotNumber > 1) // Se for um jogador dos slots de cima, virar o painel de jogo
            return true;
        return false;
    }

    private void PassTurn()
    {
        //players[activePlayerNumber].transform.SetParent(playerPanel.transform);
        if (players[_activePlayerNumber].gameObject.activeSelf) players[_activePlayerNumber].EnterGameplay(false);
        if (_activePlayerNumber == players.Count - 1) // �ltimo jogador, terminar round
        {
            _activePlayerNumber = 0;
            //MetricsAPI.EndStage();
            StartCoroutine(RoundStartCoroutine());
            return;
        }

        _activePlayerNumber++;
        StartCoroutine(TurnStartCoroutine());
    }

    public void CallAnswer(bool right, bool secondWrongAnswer = false)
    {
        if (right)
        {
            //AudioSystem.Instance.PlaySilenceableNarration(turnEndNarration);
            confirmAnswerPanel.gameObject.SetActive(true);
            confirmAnswerPanel.CallRightAnswer();
            rightAnswerEvent.Invoke();
        }
        else if(!right)
        {
            if (!secondWrongAnswer)
            {
                //EVENTO ERRO
                confirmAnswerPanel.gameObject.SetActive(true);
                confirmAnswerPanel.CallWrongAnswer();
                wrongAnswerEvent.Invoke();
            }else if (secondWrongAnswer)
            {
                //AudioSystem.Instance.PlaySilenceableNarration(turnEndNarration);
                confirmAnswerPanel.gameObject.SetActive(true);
                confirmAnswerPanel.CallSecondWrongAnswer();
                twoWrongAnswersEvent.Invoke();
            }
        }
    }

    public void PlayerGuessedRight()
    {
        rightAnswerAchieved = true;
        if (autoMetricsExperimental) MetricsAPI.EndEvent(AvatarAPI.PlayersIdsFromSlot(activePlayerForMetrics), "Acertou.", "Acertou.");


        feedImage.GetComponent<SpriteRenderer>().sprite = spritesfeed[0];
        feedtext.GetComponent<SpriteRenderer>().sprite = textosFeed[0];
        gameManager.DispararConfetes();

        goFeed.SetActive(true);
        //EVENTO CONTINUAR PÓS ACERTO
        continueAfterRightAnswerEvent.Invoke();

    }

    public void PlayerGuessedWrong(bool pCanRetry = true)
    {
        AddToErrorCounter();
        // Tentar de novo
        if (autoMetricsExperimental) MetricsAPI.EndEvent(AvatarAPI.PlayersIdsFromSlot(activePlayerForMetrics), "Acertou.", "Errou.");

        //pCanRetry = false;
        feedImage.GetComponent<SpriteRenderer>().sprite = spritesfeed[1];
        goFeed.SetActive(true);


        if (pCanRetry)
        {
            //EVENTO TENTAR NOVAMENTE

            feedtext.GetComponent<SpriteRenderer>().sprite = textosFeed[2];
           
            TryAgainEvent.Invoke();
            if (TimerSystem.Instance.isTimerEnabled)
            {
                TimeBar.instance.RestartTimer();
                TimeBar.instance.StartTimer();
            }
        }
        else
        {
            feedtext.GetComponent<SpriteRenderer>().sprite = textosFeed[1];
            PlayerGuessedWrongSecondTime();
        }
    }

    public void PlayerGuessedWrongSecondTime()
    {
        ContinueAfterSecondWrongAnswer.Invoke();
        rightAnswerAchieved = true;
    }

    public void StartTimer()
    {
        //IF you are debbuging this, the timer in settings is probably off!
        if (!TimerSystem.Instance.isTimerEnabled)
        {
            StartTimerEvent.Invoke();
        }
        else
        {
            StartTimerEvent.Invoke();
            TimeBar.instance.StartTimer();
        }
    }
    
    public void EndTimer()
    {
        TimerEndEvent.Invoke();
    }

    private int GetCurrentPlayer()
    {
        return _activePlayerNumber;
    }

    public int GetCurrentPlayerForMetrics()
    {
        return activePlayerForMetrics;
    }

    public void AddToErrorCounter()
    {
        //XGH total
        //print("Erro adicionado ao jogador no slot " + activePlayerSlot);
        switch (GetCurrentPlayer())
        {
            case 0:
                player0errors++;
                break;
            case 1:
                player1errors++;
                break;
            case 2:
                player2errors++;
                break;
            case 3:
                player3errors++;
                break;
        }
    }

    public void SpawnErrorHolder()
    {
        var errorHolderObj = new GameObject("ErrorHolder");
        errorHolderObj.AddComponent<ErrorHolder>();
        var errors = errorHolderObj.GetComponent<ErrorHolder>();
        errors.player0errors = player0errors;
        errors.player1errors = player1errors;
        errors.player2errors = player2errors;
        errors.player3errors = player3errors;
        DontDestroyOnLoad(errorHolderObj);
    }
  
}