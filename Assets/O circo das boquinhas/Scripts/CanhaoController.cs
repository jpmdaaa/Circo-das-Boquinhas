using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CanhaoController : MonoBehaviour
{
    [Header("Referências")]
    public Animator animator;
    private RoundManager roundManager;

   
    [Header("Disparo")]
    public bool podeDisparar = true;
    public bool modoTutorial = true;

    public bool doisDisparos = false;
    public bool acertou;

    [Header("Tiros")]
    public GameObject prefabTiroCerto;
    public GameObject prefabTiroErrado;
    public Transform spawnPoint; 
    

    [Header("Mira")]
    public GameObject prefabMira;         
    private GameObject miraInstanciada;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public Button botaoAudioBoquinha;
    private GameBack btnback;

    [Header("Áudios")]
    public AudioClip audioCanhao;
    public AudioClip somTiroCorreto;
    public AudioClip somTiroErrado;
    public AudioSource audioFonte;
  

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        animator = GetComponent<Animator>();
        btnback = (GameBack)GameObject.FindObjectOfType(typeof(GameBack));
        audioFonte  = (AudioSource)GameObject.FindObjectOfType(typeof(AudioSource));

        if (SceneManager.GetActiveScene().name== "RoundManager_Sample")
        {
            roundManager = (RoundManager)GameObject.FindObjectOfType(typeof(RoundManager));
        }
        

    }

    private void Update()
    {
      
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.name);
        }

        if (!modoTutorial && Input.GetMouseButtonDown(0) && podeDisparar )
        {
            if(!btnback.activate && !MouseEstaSobreUIRelevante())
            {
                Disparar(Input.mousePosition);
            }
           
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
    public void AtivarMira()
    {
        podeDisparar = true;
    }

    public void Disparar(Vector2 posicaoAlvo)
    {
        if (!podeDisparar) return;
        podeDisparar = false;

        RotacionarCanhao(posicaoAlvo);

        // Verifica acerto
        acertou = VerificarAcerto(posicaoAlvo);

        // Instanciar efeito
        Vector3 posicaoZcorrigida = new Vector3(posicaoAlvo.x, posicaoAlvo.y, 5f);
        GameObject prefab = acertou ? prefabTiroCerto : prefabTiroErrado;
        GameObject tiroInstanciado = Instantiate(prefab, posicaoZcorrigida, Quaternion.identity);
        


        if(acertou)
        {
            roundManager.CallAnswer(true);
            roundManager.PlayerGuessedRight();
            StartCoroutine(DestruirTiroDepoisDoFeedback(tiroInstanciado)); 
            Debug.Log("Acertou");
            audioFonte.Stop();
            audioFonte.PlayOneShot(somTiroCorreto);
        }

        else 
        {
            roundManager.CallAnswer(false);
            roundManager.PlayerGuessedWrong();
            StartCoroutine(DestruirTiroDepoisDoFeedback(tiroInstanciado));
            Debug.Log("Errou");
            audioFonte.Stop();
            audioFonte.PlayOneShot(somTiroErrado);

        }
        /*
        if(doisDisparos)
        {
            podeDisparar = true;
            //roundManager.CallAnswer(false);
            roundManager.PlayerGuessedWrongSecondTime();
            StartCoroutine(DestruirTiroDepoisDoFeedback(tiroInstanciado));
            doisDisparos = false;
            audioFonte.Stop();
            audioFonte.PlayOneShot(somTiroErrado);
        }
        animator.Play("CanhaoIdle", 0);
        */
    }
    private IEnumerator DestruirTiroDepoisDoFeedback(GameObject tiro)
    {
        yield return new WaitForSeconds(1f); // tempo de feedback na tela
        Destroy(tiro);
    }
    private void RotacionarCanhao(Vector2 posicaoAlvo)
    {
        // Converte a posição do mouse de tela para mundo
        Vector2 posMundo = Camera.main.ScreenToWorldPoint(posicaoAlvo);

        // Calcula a direção a partir da posição do canhão
        Vector2 direcao = (posMundo - (Vector2)transform.position).normalized;

        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        string trigger = DirecaoParaAnimacao(angulo);

    
        animator.SetTrigger(trigger);
    }
    private string DirecaoParaAnimacao(float angulo)
    {
        // Normalize o ângulo para o intervalo [-180, 180]
        if (angulo > 180f) angulo -= 360f;

        // Direita: -45° a +45°
        // Frente: +45° a +135°
        // Esquerda: >= +135° ou <= -135°
        if (angulo  <= 80)
            return "Disparar_Direita";
        else if (angulo >= 111f )
            return "Disparar_Esquerda";
        else
            return "Disparar_Frente";
    }


    public void DisparoTutorial()
    {
        Debug.Log("Disparei canhao tutorial");
        GuessItem[] itens = GameObject.FindObjectsOfType<GuessItem>();

        if (TutorialManager.Instance == null) return;

        var modo = TutorialManager.Instance.modoAtual;

        if (modo == DifficultyLevels.Figures)
        {
            // Disparar em dois corretos
            List<GuessItem> alvosCorretos = new List<GuessItem>();
            foreach (var item in itens)
            {
                if (item.isRespostaCorreta)
                    alvosCorretos.Add(item);
            }
            StartCoroutine(DispararEmSequencia(alvosCorretos));
            audioFonte.Stop();
            audioFonte.PlayOneShot(somTiroCorreto);
        }
        else
        {
            // Disparar apenas uma vez no correto
            foreach (var item in itens)
            {
                if (item.isRespostaCorreta)
                {
                    Vector3 pos = item.transform.position;
                    pos.z = 5f;

                    RotacionarCanhaoMundo(pos);
                    InstanciarMira(pos);

                    if (prefabTiroCerto != null)
                        Instantiate(prefabTiroCerto, pos, Quaternion.identity);
                    break;
                }
            }
        }
    }
    private bool VerificarAcerto(Vector2 telaPosicao)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = telaPosicao;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            GuessItem item = result.gameObject.GetComponent<GuessItem>();
            if (item != null)
                return item.isRespostaCorreta;
        }

        return false;
    }
    private IEnumerator DispararEmSequencia(List<GuessItem> alvos)
    {
        foreach (var alvo in alvos)
        {
            if (alvo == null) continue;

            Vector3 pos = alvo.transform.position;
            pos.z = 5f; 

            RotacionarCanhao(pos);
            InstanciarMira(pos);

            if (prefabTiroCerto != null)
                Instantiate(prefabTiroCerto, pos, Quaternion.identity);

            yield return new WaitForSeconds(1.5f);
        }
    }

    private void InstanciarMira(Vector2 destino)
    {
        if (prefabMira == null || spawnPoint == null) return;

        if (miraInstanciada != null)
            Destroy(miraInstanciada); // remove a anterior se houver

        miraInstanciada = Instantiate(prefabMira, spawnPoint.position, Quaternion.identity);

        Vector2 direcao = destino - (Vector2)spawnPoint.position;
        float distancia = direcao.magnitude;

        miraInstanciada.transform.right = direcao.normalized;
        miraInstanciada.transform.position = spawnPoint.position + (Vector3)(direcao / 2f); // centraliza
        //miraInstanciada.transform.localScale = new Vector3(distancia, 1f, 1f); // estica a linha
    }
    private bool MouseEstaSobreUIRelevante()
    {
        if (raycaster == null || eventSystem == null)
        {
            Debug.LogWarning("Raycaster ou EventSystem não atribuídos.");
            return false;
        }

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject == null) continue;

            // Verifica se o objeto clicado é filho do botão de áudio
            if (botaoAudioBoquinha != null && result.gameObject.transform.IsChildOf(botaoAudioBoquinha.transform))
            {
                return true;
            }

            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
            // Alternativa: checar por tag "UIIgnorarTiro"
            if (result.gameObject.CompareTag("UIIgnorarTiro"))
            {
                return true;
            }
        }

        return false;
    }


    private void RotacionarCanhaoMundo(Vector3 posicaoMundo)
    {
        Vector2 direcao = ((Vector2)posicaoMundo - (Vector2)transform.position).normalized;

        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        string trigger = DirecaoParaAnimacao(angulo);

        animator.SetTrigger(trigger);
    }


}
