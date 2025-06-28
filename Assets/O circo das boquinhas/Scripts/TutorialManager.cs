using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Boquinhas.Core;
using Boquinhas.ConfigurationSettings;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;


    public DifficultyLevels modoAtual;
    private DifficultySaveManager difficulty;

    [Header("UI Principal")]
    
    public GameObject text_modo_Bocas;
    public GameObject text_modo_Letas;
    public GameObject text_modo_Figuras;

    public GameObject FeedbackPosi;

    private Sprite spriteDaBoquinhaApresentada;
    private Dictionary<Sprite, Sprite> mapaLetraParaBoquinha = new Dictionary<Sprite, Sprite>();



    public Button botaoPular;
    public GameObject maoCanhao;
    public GameObject pnlPopup;

    [Header("Painéis de Imagens")]
    public GameObject painelFiguras;       // Ativa no modo Figures
    public GameObject painelBocasLetras;   // Ativa nos outros modos
    public List<Image> iconesPainelCentral; // 6 imagens (3 para boquinhas/letras ou 6 para figuras)


    [Header("Faixas por Modo")]
    public Animator faixaAnimatorBoquinhas;
    public Animator faixaAnimatorLetras;
    public Animator faixaAnimatorFiguras;
  

    [Header("Ícone de Fala do Coelho")]
    public GameObject imageFalaCoelho;

    //[Header("Ícones do Painel de Jogo (centro)")]
    //public Image iconePainel1;
    //public Image iconePainel2;
    //public Image iconePainel3;
    public int mode;
 
    [Header("Listas de Sprites Recebidas")]
    public List<Sprite> boquinhasDisponiveis;
    public List<Sprite> letrasDisponiveis;
    public List<Sprite> figurasDisponiveis;

    [Header("Animações")]
    public Animator cortinaEsquerdaAnimator;
    public Animator cortinaDireitaAnimator;
    

    [Header("Áudios")]
    public AudioSource audioFonte;
    public List<AudioClip> audiosBoquinhas;
    public List<AudioClip> audiosLetras;
    public List<AudioClip> audiosFiguras;
    private Dictionary<Sprite, AudioClip> mapaBoca = new();
    private Dictionary<Sprite, Sprite> mapaLetra = new();
    private Dictionary<Sprite, AudioClip> mapaFigura = new();
    public AudioClip somCortina;
    public AudioClip somFaixa;
    public AudioClip somAgoraEhComVoce;

    [Header("Boquinha Correta")]
    private SpriteRenderer boquinhaCorreta;
    public GameObject GO_boquinhaCorreta;

    [Header("Controle Geral")]
    //[SerializeField] private GameObject canvasBloqueador;
  
    public RoundManager roundManager;
    public CanhaoController canhao;

    [Header("Coelho")]
    public GameObject coelho;
    private Transform coelhoTransform;
    private Animator coelhoAnimator;
    public Vector3 posicaoInicialCoelho = new Vector3(5f, -1.31f, 1.957962f);
    public Vector3 posicaoFinalCoelho = new Vector3(4.731943f, -0.6551708f, 1.957962f);


    [Header("Confetes")]
    public List<Sprite> confeteSprites; // lista de cores
    public GameObject confetePrefab;    // prefab com Rigidbody2D
    public Transform confeteParent;     // container opcional para manter organizado
    public int quantidadeConfetes = 20;

    private void Awake()
    {
        Instance = this;
        CriarMapaBoca();
        CriarMapaLetra();
        CriarMapaFigura();

        boquinhaCorreta = GO_boquinhaCorreta.GetComponent<SpriteRenderer>();
        difficulty = (DifficultySaveManager)GameObject.FindObjectOfType(typeof(DifficultySaveManager));
        modoAtual = difficulty.GetSavedDifficultyLevel();
        coelhoAnimator = coelho.GetComponent<Animator>();
        coelhoTransform = coelho.GetComponent<Transform>();
        text_modo_Bocas.SetActive(false);
        text_modo_Letas.SetActive(false);
        text_modo_Figuras.SetActive(false);
      
        confetePrefab.SetActive(false);
        FeedbackPosi.SetActive(false);
        maoCanhao.SetActive(false);
        botaoPular.gameObject.SetActive(true);
        imageFalaCoelho.SetActive(false);
        GO_boquinhaCorreta.SetActive(false);
        CriarMapaLetraBoquinha();
        coelhoTransform.position = posicaoInicialCoelho;
        if (audioFonte == null)
        {
            audioFonte = GetComponent<AudioSource>();
            if (audioFonte == null)
                Debug.LogError("AudioSource não encontrado no TutorialManager.");
        }

        StartCoroutine(ExecutarTutorial(modoAtual));

    }

    private IEnumerator ExecutarTutorial(DifficultyLevels modo)
    {
        Debug.Log("Executar tutorial");    
       

        switch (modoAtual)
        {
            case DifficultyLevels.Mouths:
                yield return StartCoroutine(TutorialBoquinhas());
                Debug.Log("Modo Boquinhas selecionado");
                break;
            case DifficultyLevels.Letters:
                yield return StartCoroutine(TutorialLetras());
                Debug.Log("Modo Letras selecionado");
                break;
            case DifficultyLevels.Figures:
                yield return StartCoroutine(TutorialFiguras());
                Debug.Log("Modo Figuras selecionado");
                break;
        }
   
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            IrParaJogo();
        }

    }
    private IEnumerator TutorialBoquinhas()
    {
        text_modo_Bocas.SetActive(true);
        painelFiguras.SetActive(false);
        painelBocasLetras.SetActive(true);

        List<Sprite> sorteados = SortearAleatorios(boquinhasDisponiveis, 3);
        int indexCorreto = Random.Range(0, sorteados.Count);

        AtualizarIconesPainel(sorteados, indexCorreto);

        Sprite resposta = sorteados[indexCorreto];
        AudioClip audioResposta = null; // ✅ CORREÇÃO AQUI

        if (boquinhaCorreta != null && resposta != null)
        {
            boquinhaCorreta.sprite = resposta;
            if (mapaBoca.TryGetValue(resposta, out AudioClip clip))
                audioResposta = clip;
        }

        yield return StartCoroutine(EtapasFinaisTutorial(audioResposta));
    }

    private IEnumerator TutorialLetras()
    {
        text_modo_Letas.SetActive(true);
        painelFiguras.SetActive(false);
        painelBocasLetras.SetActive(true);

        List<Sprite> sorteados = SortearAleatorios(letrasDisponiveis, 3);
        int indexCorreto = Random.Range(0, sorteados.Count);

        AtualizarIconesPainel(sorteados, indexCorreto);

        Sprite letraCorreta = sorteados[indexCorreto];
        AudioClip audioResposta = null;

        if (mapaLetra.TryGetValue(letraCorreta, out Sprite boca))
        {
            boquinhaCorreta.sprite = boca;
            if (mapaBoca.TryGetValue(boca, out AudioClip clip))
                audioResposta = clip;
        }

        else
        {
            Debug.LogWarning("Boquinha não encontrada para a letra: " + letraCorreta.name);
        }

        yield return StartCoroutine(EtapasFinaisTutorial(audioResposta));
    }

    private IEnumerator TutorialFiguras()
    {
        Debug.Log("tutorial figurinhas");
        text_modo_Figuras.SetActive(true);
        painelFiguras.SetActive(true);
        painelBocasLetras.SetActive(false);

        List<Sprite> sorteados = SortearAleatorios(figurasDisponiveis, 9);
        int indexCorreto = Random.Range(3, 9);

        AtualizarIconesPainelComIndiceCorreto(sorteados, indexCorreto);

        string chave = NormalizarNome(sorteados[indexCorreto].name);
     

        Sprite figura = sorteados[indexCorreto];
        AudioClip audioResposta = null;

        if (mapaFigura.TryGetValue(figura, out AudioClip clip))
            audioResposta = clip;


        yield return StartCoroutine(EtapasFinaisTutorial(clip));
    }

    private IEnumerator EtapasFinaisTutorial(AudioClip audioResposta = null)
    {
        yield return new WaitForSeconds(2.0f);
        AtivarFaixa(true);

        yield return new WaitForSeconds(2.0f);
        cortinaEsquerdaAnimator?.SetTrigger("abrir");
        cortinaDireitaAnimator?.SetTrigger("abrir");
        audioFonte.PlayOneShot(somCortina);

        yield return new WaitForSeconds(2.0f);
        coelhoAnimator?.SetTrigger("aparecer");
        coelhoTransform.position = posicaoFinalCoelho;
        imageFalaCoelho.SetActive(true);
        GO_boquinhaCorreta.SetActive(true);

        // TOCAR O ÁUDIO DA RESPOSTA DEPOIS QUE O COELHO APARECE
        if (audioResposta != null)
        {
            audioFonte.Stop();
            audioFonte.clip = audioResposta;
            audioFonte.volume = 1f;
            audioFonte.mute = false;
            audioFonte.Play();
            yield return new WaitForSeconds(audioResposta.length);
        }

        yield return new WaitForSeconds(2.0f);
        maoCanhao.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        maoCanhao.SetActive(false);
        canhao.DisparoTutorial();

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FinalizarTutorial());
    }



    private List<Sprite> SortearAleatorios(List<Sprite> lista, int quantidade)
    {
        List<Sprite> copia = new List<Sprite>(lista);
        for (int i = 0; i < copia.Count; i++)
        {
            Sprite temp = copia[i];
            int rand = Random.Range(i, copia.Count);
            copia[i] = copia[rand];
            copia[rand] = temp;
        }
        return copia.GetRange(0, Mathf.Min(quantidade, copia.Count));
    }
    private void AtualizarIconesPainel(List<Sprite> sprites, int indexCorreto)
    {
        // Para modos Boquinhas e Letras (índice único passado)
        for (int i = 0; i < 3; i++)
        {
            if (i >= iconesPainelCentral.Count || i >= sprites.Count)
                continue;

            Image icone = iconesPainelCentral[i];
            icone.sprite = sprites[i];

            GuessItem guess = icone.GetComponent<GuessItem>();
            if (guess == null)
                guess = icone.gameObject.AddComponent<GuessItem>();

            guess.tipo = modoAtual.ToString();
            guess.isRespostaCorreta = (i == indexCorreto);
        }

        if (modoAtual == DifficultyLevels.Mouths)
        {
            boquinhaCorreta.sprite = sprites[indexCorreto];
        }
    }

    private void AtualizarIconesPainel(List<Sprite> sprites)
    {
        if (sprites.Count < 9) return;

        int indexCorreto1 = Random.Range(3, 9);
        int indexCorreto2;
        do
        {
            indexCorreto2 = Random.Range(3, 9);
        } while (indexCorreto2 == indexCorreto1);

        for (int i = 3; i < 9; i++)
        {
            if (i >= iconesPainelCentral.Count || i >= sprites.Count)
                continue;

            Image icone = iconesPainelCentral[i];
            icone.sprite = sprites[i];

            GuessItem guess = icone.GetComponent<GuessItem>();
            if (guess == null)
                guess = icone.gameObject.AddComponent<GuessItem>();

            guess.tipo = modoAtual.ToString();
            guess.isRespostaCorreta = (i == indexCorreto1 || i == indexCorreto2);
        }
    }


    private void CriarMapaLetraBoquinha()
    {
        mapaLetraParaBoquinha = new Dictionary<Sprite, Sprite>();

        Sprite letraA = letrasDisponiveis.Find(l => l != null && l.name == "Letra_A");
        Sprite letraB = letrasDisponiveis.Find(l => l != null && l.name == "Letra_B");
        Sprite letraC = letrasDisponiveis.Find(l => l != null && l.name == "Letra_C");

        Sprite bocaA = boquinhasDisponiveis.Find(b => b != null && b.name == "Boca_A");
        Sprite bocaB = boquinhasDisponiveis.Find(b => b != null && b.name == "Boca_B");
        Sprite bocaC = boquinhasDisponiveis.Find(b => b != null && b.name == "Boca_C");

        if (letraA != null && bocaA != null) mapaLetraParaBoquinha[letraA] = bocaA;
        if (letraB != null && bocaB != null) mapaLetraParaBoquinha[letraB] = bocaB;
        if (letraC != null && bocaC != null) mapaLetraParaBoquinha[letraC] = bocaC;
    }

    private void AtualizarIconesPainelComIndiceCorreto(List<Sprite> sprites, int indexCorreto)
    {
        for (int i = 3; i < 9; i++)
        {
            if (i >= iconesPainelCentral.Count || i >= sprites.Count)
                continue;

            Image icone = iconesPainelCentral[i];
            icone.sprite = sprites[i];

            GuessItem guess = icone.GetComponent<GuessItem>();
            if (guess == null)
                guess = icone.gameObject.AddComponent<GuessItem>();

            guess.tipo = modoAtual.ToString();
            guess.isRespostaCorreta = (i == indexCorreto); // só uma correta
        }
    }

    private void AplicarSpriteComTipo(Image icone, List<Sprite> sprites, int index, bool isCorreta)
    {
        if (sprites.Count > index && icone != null)
        {
            icone.sprite = sprites[index];
            icone.gameObject.SetActive(true);

            GuessItem guessItem = icone.GetComponent<GuessItem>();
            if (guessItem == null)
                guessItem = icone.gameObject.AddComponent<GuessItem>();

            guessItem.tipo = modoAtual.ToString(); // ex: "Mouths"
            guessItem.isRespostaCorreta = isCorreta;
        }
        else if (icone != null)
        {
            icone.sprite = null;
            icone.gameObject.SetActive(false);
        }
    }


    private void AtivarFaixa(bool desenrolar)
    {
        Animator faixa = null;

        switch (modoAtual)
        {
            case DifficultyLevels.Mouths:
                faixa = faixaAnimatorBoquinhas;
                break;
            case DifficultyLevels.Letters:
                faixa = faixaAnimatorLetras;
                break;
            case DifficultyLevels.Figures:
                faixa = faixaAnimatorFiguras;
                break;
        }

        if (faixa == null) return;

        if (desenrolar)
        {
            faixa.Play("FaixaAnim", 0, 0f);
            audioFonte.PlayOneShot(somFaixa);
            faixa.speed = 0.6f;
        }
        else
        {
            faixa.Play("FaixaAnim", 0, 1f);
           
            faixa.speed = -0.6f;
        }
    }
    private void LimparPainel(Transform painel)
    {
        foreach (Transform child in painel)
            Destroy(child.gameObject);
    }

 

    private IEnumerator FinalizarTutorial()
    {
   
        AtivarFaixa(false); // enrolar faixa
     
        yield return new WaitForSeconds(0.5f);

        text_modo_Bocas.SetActive(false);
        text_modo_Letas.SetActive(false);
        text_modo_Figuras.SetActive(false);

        FeedbackPosi.SetActive(true);
        
        botaoPular.gameObject.SetActive(false);
        //canvasBloqueador.SetActive(false);

        DispararConfetes();
        audioFonte.PlayOneShot(somAgoraEhComVoce);

        yield return new WaitForSeconds(4f);

        IrParaJogo();
        
    }


    //para chamar no botao de pular
    public void PularTutorial()
    {
        StartCoroutine(FinalizarTutorial());
        Debug.Log("Pulei tutorial");
        
    }
    private void DispararConfetes()
    {
    
        for (int i = 0; i < quantidadeConfetes; i++)
        {

            Vector3 posicaoSpawn = new Vector3(
                Random.Range(-3.5f, 3.5f),         // espalha mais horizontalmente
                5f + Random.Range(0f, 2.5f),       // pequena variação vertical
                5f                                 // sempre na frente
            );

            GameObject confete = Instantiate(confetePrefab, posicaoSpawn, Quaternion.identity, confeteParent);
            confete.SetActive(true);


            SpriteRenderer sr = confete.GetComponent<SpriteRenderer>();
            if (sr != null && confeteSprites.Count > 0)
            {
                sr.sprite = confeteSprites[Random.Range(0, confeteSprites.Count)];
            }

            Rigidbody2D rb = confete.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forcaX = Random.Range(-4.5f, 4.5f); // mais variação horizontal
                float forcaY = Random.Range(3f, 8f);      // confetes mais animados
                rb.AddForce(new Vector2(forcaX, forcaY), ForceMode2D.Impulse);

                // opcional: variação na rotação para mais realismo
                float torque = Random.Range(-5f, 5f);
                rb.AddTorque(torque, ForceMode2D.Impulse);
            }
            
        }
    }

    private void IrParaJogo()
    {
        string jogo = GameManager.Instance.boquinhasConfigurationSettings.gameSceneName;
        FindObjectOfType<ScenesSystem>().ChangeScene(jogo);
    }
    private void CriarMapaBoca()
    {
        mapaBoca.Clear();
        foreach (var boca in boquinhasDisponiveis)
        {
            string chave = NormalizarNome(boca.name);
            var audio = audiosBoquinhas.Find(a => NormalizarNome(a.name) == chave);
            if (audio != null)
                mapaBoca[boca] = audio;
            else
                Debug.LogWarning($"Áudio da boca '{boca.name}' não encontrado.");
        }
    }
    private void CriarMapaLetra()
    {
        mapaLetra.Clear();
        foreach (var letra in letrasDisponiveis)
        {
            string nomeLetra = letra.name;
            var nomeBoca = nomeLetra; // ou mapeie manualmente se necessário

            Sprite boca = boquinhasDisponiveis.Find(b => b.name == nomeBoca);
            if (boca != null)
                mapaLetra[letra] = boca;
            else
                Debug.LogWarning($"Boca '{nomeBoca}' não encontrada para a letra '{nomeLetra}'.");
        }
    }


    private void CriarMapaFigura()
    {
        mapaFigura.Clear();
        foreach (var figura in figurasDisponiveis)
        {
            string chave = NormalizarNome(figura.name);
            var audio = audiosFiguras.Find(a => NormalizarNome(a.name) == chave);
            if (audio != null)
                mapaFigura[figura] = audio;
            else
                Debug.LogWarning($"Áudio da figura '{figura.name}' não encontrado.");
        }
    }


    private string NormalizarNome(string nome)
    {
        string normalizado = nome.ToLower().Normalize(System.Text.NormalizationForm.FormD);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (char c in normalizado)
        {
            var categoria = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (categoria != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }
        }

        return sb.ToString()
                 .Replace("(", "")
                 .Replace(")", "")
                 .Replace("-", "")
                 .Replace(" ", "")
                 .Trim();
    }

}
