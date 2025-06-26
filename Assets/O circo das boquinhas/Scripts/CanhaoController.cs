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

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(SceneManager.GetActiveScene().name== "RoundManager_Sample")
        {
            roundManager = (RoundManager)GameObject.FindObjectOfType(typeof(RoundManager));
        }
        

    }

    private void Update()
    {
        if (!modoTutorial && Input.GetMouseButtonDown(0) && podeDisparar)
        {
            Disparar(Input.mousePosition);
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
          
        }

        else 
        {
            roundManager.CallAnswer(false);
            roundManager.PlayerGuessedWrong();
            StartCoroutine(DestruirTiroDepoisDoFeedback(tiroInstanciado));
            Debug.Log("Errou");
           
        }

        if(doisDisparos)
        {
            podeDisparar = true;
            roundManager.CallAnswer(false);
            roundManager.PlayerGuessedWrongSecondTime();
            StartCoroutine(DestruirTiroDepoisDoFeedback(tiroInstanciado));
            doisDisparos = false;
        }

    }
    private IEnumerator DestruirTiroDepoisDoFeedback(GameObject tiro)
    {
        yield return new WaitForSeconds(1f); // tempo de feedback na tela
        Destroy(tiro);
    }
    private void RotacionarCanhao(Vector2 posicaoAlvo)
    {
        animator.ResetTrigger("Disparar_Direita");
        animator.ResetTrigger("Disparar_Esquerda");
        animator.ResetTrigger("Disparar_Frente");

        animator.Play("CanhaoIdle", 0);

        Vector2 direcao = (posicaoAlvo - (Vector2)transform.position).normalized;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        string trigger = DirecaoParaAnimacao(angulo);
        animator.SetTrigger(trigger);
    }

    private string DirecaoParaAnimacao(float angulo)
    {
        float deltaX = Mathf.Cos(angulo * Mathf.Deg2Rad);

        if (Mathf.Abs(deltaX) < 0.1f)
            return "Disparar_Frente";
        else if (deltaX > 0)
            return "Disparar_Direita";
        else
            return "Disparar_Esquerda";
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

                    RotacionarCanhao(pos);
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

}
