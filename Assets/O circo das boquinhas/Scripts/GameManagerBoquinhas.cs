using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Boquinhas.Core;
using UnityEngine.SceneManagement;

public class GameManagerBoquinhas : MonoBehaviour
{
    [Header("Game Mode")]
    public DifficultyLevels modoAtual;
    private DifficultySaveManager difficulty;
    private RoundManager roundManager;
    private int numeroJogadores;
    private int numeroDaRodada => roundManager != null ? roundManager.GetCurrentRoundNumber() : 1;

    [Header("UI")]

    public List<Image> iconesPainel_6;
    public List<Image> iconesPainel_8;
    public List<Image> iconesPainel_12;
    public GameObject figuras6GO;
    public GameObject figuras8GO;
    public GameObject figuras12GO;

    [Header("Coelho")]
    public GameObject coelho;
    public Animator coelhoAnimator;
    public GameObject imageFalaCoelho;
    public Transform coelhoTransform;
    public Vector3 posicaoInicialCoelho = new Vector3(5f, -1.31f, 1.957962f);
    public Vector3 posicaoFinalCoelho = new Vector3(4.731943f, -0.6551708f, 1.957962f);

    public GameObject GO_boquinhaCorreta;
    private SpriteRenderer boquinhaCorreta;

    [Header("Cortinas")]
    public Animator cortinaEsquerdaAnimator;
    public Animator cortinaDireitaAnimator;

    [Header("Listas de Sprites")]
    public List<Sprite> boquinhasDisponiveis;
    public List<Sprite> letrasDisponiveis;
    public List<Sprite> figurasDisponiveis;

    [Header("Referências")]
    public CanhaoController canhao;

    [Header("Confetes")]
    public List<Sprite> confeteSprites; // lista de cores
    public GameObject confetePrefab;    // prefab com Rigidbody2D
    public Transform confeteParent;     // container opcional para manter organizado
    public int quantidadeConfetes = 30;

    [Header("Áudios")]
    public List<AudioClip> audiosBoquinhas;
    public AudioSource audioSource;
    private AudioClip audioBoquinhaCorreta;
    public List<AudioClip> audiosFiguras;
    private Dictionary<string, AudioClip> mapaAudioBoquinhas = new();
    private Dictionary<string, AudioClip> mapaAudioFiguras = new();
    public AudioClip somCortina;
    private AudioClip somFeedback;


    private Dictionary<Sprite, Sprite> mapaLetraParaBoquinha = new();
    Dictionary<string, string> mapaLetrasParaBocas = new Dictionary<string, string>
{
    {"A", "A"},
    {"ASA", "Z-ASA"},
    {"B", "B"},
    {"C", "C-QU"},
    {"CE", "S-CE-CI-SC"},
    {"CI", "S-CE-CI-SC"},
    {"CH", "X-CH"},
    {"D", "D"},
    {"E", "É"},
    {"F", "F"},
    {"G", "G-GU"},
    {"GE", "J-GE-GI"},
    {"GI", "J-GE-GI"},
    {"GU", "G-GU"},
    {"H", "A"},        // H é mudo, pode usar boca neutra como 'A' ou 'Z-ASA'
    {"I", "I"},
    {"J", "J-GE-GI"},
    {"L", "L"},
    {"LH", "LH"},
    {"M", "M"},
    {"N", "N"},
    {"NH", "NH"},
    {"O", "Ó"},
    {"P", "P"},
    {"Q", "C-QU"},
    {"QU", "C-QU"},
    {"R", "R-RR"},
    {"RR", "R-RR"},
    {"S", "S-CE-CI-SC"},
    {"SS", "S-CE-CI-SC"},
    {"T", "T"},
    {"U", "U"},
    {"V", "V"},
    {"X", "X-CH"},
    {"Z", "Z-ASA"},
    {"Ã", "Ã"},
    {"Ç", "S-CE-CI-SC"},
    {"É", "É"},
    {"Ó", "Ó"}
};



    private void Start()
    {
        coelhoTransform = coelho.GetComponent<Transform>();
        coelhoTransform.position = posicaoInicialCoelho;
        coelhoAnimator = coelho.GetComponent<Animator>();
        boquinhaCorreta = GO_boquinhaCorreta.GetComponent<SpriteRenderer>();
        difficulty = (DifficultySaveManager)GameObject.FindObjectOfType(typeof(DifficultySaveManager));
        modoAtual = difficulty.GetSavedDifficultyLevel();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "RoundManager_Sample")
        {
            roundManager = (RoundManager)GameObject.FindObjectOfType(typeof(RoundManager));
        }
        CriarMapaAudioBoquinhas();
        CriarMapaAudioFiguras();
        CriarMapaLetraBoquinha();
        PrepararRodada();
      

    }

    public IEnumerator IniciarGameplay()
    {
        
        yield return new WaitForSeconds(2.0f);

        cortinaEsquerdaAnimator?.SetTrigger("abrir");
        cortinaDireitaAnimator?.SetTrigger("abrir");
        audioSource.PlayOneShot(somCortina);

        yield return new WaitForSeconds(2.0f);

        coelhoAnimator?.SetTrigger("aparecer");
        coelhoTransform.position = posicaoFinalCoelho;
        imageFalaCoelho.SetActive(true);
        GO_boquinhaCorreta.SetActive(true);

        yield return new WaitForSeconds(0.5f); // dá um tempinho antes de falar

        // Agora sim toca o áudio
        PlayAudioBoquinha();

        canhao.podeDisparar = true;
        yield return new WaitForSeconds(1.5f);

    }
    public void PrepararRodada()
    {
        int quantidade = ObterNumeroDeImagens();

        // Define a lista correta de acordo com o modo atual
        List<Sprite> fonte = modoAtual switch
        {
            DifficultyLevels.Mouths => boquinhasDisponiveis,
            DifficultyLevels.Letters => letrasDisponiveis,
            DifficultyLevels.Figures => figurasDisponiveis,
            _ => new List<Sprite>()
        };

        // Sorteia os sprites
        List<Sprite> sorteados = SortearAleatorios(fonte, quantidade);

        // Ativa o painel correspondente à quantidade
        figuras6GO.SetActive(quantidade == 6);
        figuras8GO.SetActive(quantidade == 8);
        figuras12GO.SetActive(quantidade == 12);

        // Escolhe índice da resposta correta
        int idx = Random.Range(0, sorteados.Count);

        // Atualiza o painel com os sprites e define a resposta correta
        AtualizarIconesPainel(sorteados, idx);
        // Define boquinha correta e áudio conforme o modo
        if (modoAtual == DifficultyLevels.Letters && mapaLetraParaBoquinha.TryGetValue(sorteados[idx], out Sprite boca))
        {
            boquinhaCorreta.sprite = boca;
            string chave = NormalizarNome(boca.name);
            if (mapaAudioBoquinhas.TryGetValue(chave, out AudioClip clip))
            {
                audioBoquinhaCorreta = clip;
               
            }
            else
            {
                Debug.LogWarning($"Áudio não encontrado para letra '{chave}'");
            }
        }
        else if (modoAtual == DifficultyLevels.Mouths)
        {
            boquinhaCorreta.sprite = sorteados[idx];
            string chave = NormalizarNome(sorteados[idx].name);
            if (mapaAudioBoquinhas.TryGetValue(chave, out AudioClip clip))
            {
                audioBoquinhaCorreta = clip;
             
            }
            else
            {
                Debug.LogWarning($"Áudio não encontrado para boquinha '{chave}'");
            }
        }
        else if (modoAtual == DifficultyLevels.Figures)
        {
            string chave = NormalizarNome(sorteados[idx].name);
            if (mapaAudioFiguras.TryGetValue(chave, out AudioClip clip))
            {
                audioBoquinhaCorreta = clip;
           
            }
            else
            {
                Debug.LogWarning($"Áudio não encontrado para figura '{chave}'");
            }
        }



    }


    private void LimparTodosOsIcones()
    {
        void Limpar(Transform grupo)
        {
            foreach (Transform t in grupo)
            {
                var img = t.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                    img.sprite = null;
            }
        }

        Limpar(figuras6GO.transform);
        Limpar(figuras8GO.transform);
        Limpar(figuras12GO.transform);
    }

    private int ObterNumeroDeImagens()
    {
        if (numeroJogadores <= 2)
        {
            if (numeroDaRodada <= 2) return 6;
            else if (numeroDaRodada <= 4) return 8;
            else return 12;
        }
        else
        {
            if (numeroDaRodada <= 2) return 6;
            else return 12;
        }
    }
    private void AtualizarIconesPainel(List<Sprite> sprites, int respostaCorretaIndex)
    {
        int total = sprites.Count;
     
        // Escolhe a lista de ícones correta com base na quantidade de sprites
        List<Image> iconesAtuais = total switch
        {
            6 => iconesPainel_6,
            8 => iconesPainel_8,
            12 => iconesPainel_12,
            _ => new List<Image>() // fallback vazio
        };

        for (int i = 0; i < iconesAtuais.Count; i++)
        {
            var icone = iconesAtuais[i];

            if (i < total)
            {
                icone.gameObject.SetActive(true);
                icone.sprite = sprites[i];

                var guess = icone.GetComponent<GuessItem>();
                if (guess == null)
                    guess = icone.gameObject.AddComponent<GuessItem>();

                guess.tipo = modoAtual.ToString();
                guess.isRespostaCorreta = (i == respostaCorretaIndex);
            }
            else
            {
                icone.sprite = null;
                var guess = icone.GetComponent<GuessItem>();
                if (guess != null)
                {
                    guess.isRespostaCorreta = false;
                    guess.tipo = "";
                }
            }
        }
    }



    private List<Sprite> SortearAleatorios(List<Sprite> lista, int quantidade)
    {
        List<Sprite> copia = new(lista);
        for (int i = 0; i < copia.Count; i++)
        {
            Sprite temp = copia[i];
            int rand = Random.Range(i, copia.Count);
            copia[i] = copia[rand];
            copia[rand] = temp;
        }
        return copia.GetRange(0, Mathf.Min(quantidade, copia.Count));
    }
    public void CriarMapaLetraBoquinha()
    {
        mapaLetraParaBoquinha.Clear();

        foreach (Sprite letra in letrasDisponiveis)
        {
            string nomeLetra = letra.name;

            if (mapaLetrasParaBocas.TryGetValue(nomeLetra, out string nomeBoquinha))
            {
                Sprite boquinha = boquinhasDisponiveis.Find(b => b.name == nomeBoquinha);
                if (boquinha != null)
                {
                    mapaLetraParaBoquinha[letra] = boquinha;
                }
                else
                {
                    Debug.LogWarning($"Boquinha '{nomeBoquinha}' não encontrada para a letra '{nomeLetra}'");
                }
            }
            else
            {
                Debug.LogWarning($"Letra '{nomeLetra}' não está mapeada para nenhuma boquinha.");
            }
        }
    }

    private void CriarMapaAudioBoquinhas()
    {
        mapaAudioBoquinhas.Clear();
        foreach (var clip in audiosBoquinhas)
        {
            string chave = NormalizarNome(clip.name);
            if (!mapaAudioBoquinhas.ContainsKey(chave))
                mapaAudioBoquinhas[chave] = clip;
        }
    }

    private void CriarMapaAudioFiguras()
    {
        mapaAudioFiguras.Clear();
        foreach (var clip in audiosFiguras)
        {
            string chave = NormalizarNome(clip.name);
            if (!mapaAudioFiguras.ContainsKey(chave))
                mapaAudioFiguras[chave] = clip;
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


    public void DispararConfetes()
    {

        for (int i = 0; i < quantidadeConfetes; i++)
        {

            Vector3 posicaoSpawn = new Vector3(
                Random.Range(-3.0f, 3.0f),         // espalha mais horizontalmente
                5f + Random.Range(0f, 2.5f),       // pequena variação vertical
                1000f                                 // sempre na frente
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
                float forcaX = Random.Range(-4.0f, 4.0f); // mais variação horizontal
                float forcaY = Random.Range(3f, 8f);      // confetes mais animados
                rb.AddForce(new Vector2(forcaX, forcaY), ForceMode2D.Impulse);

                // opcional: variação na rotação para mais realismo
                float torque = Random.Range(-5f, 5f);
                rb.AddTorque(torque, ForceMode2D.Impulse);
            }
            StartCoroutine(DestruirQuandoForaDaTela(confete));
        }

    }
    private IEnumerator DestruirQuandoForaDaTela(GameObject confete)
    {
        yield return new WaitForSeconds(3f); // aguarda confete "voar"

        while (confete != null)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(confete.transform.position);
            bool foraDaTela = viewPos.y < 0 || viewPos.y > 1 || viewPos.x < 0 || viewPos.x > 1;

            if (foraDaTela)
            {
                Destroy(confete);
                yield break;
            }

            yield return null;
        }
    }
    public void PlayAudioBoquinha()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource não está definido.");
            return;
        }

        if (audioBoquinhaCorreta == null)
        {
            Debug.LogWarning("Áudio da boquinha correta está nulo.");
            return;
        }

        audioSource.Stop();
        audioSource.clip = audioBoquinhaCorreta;
        audioSource.volume = 1f;
        audioSource.mute = false;
        audioSource.Play();
    }


}
