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
    public Vector3 posicaoInicialCoelho = new Vector3(5f, +1f, 1000f);
    public Vector3 posicaoFinalCoelho = new Vector3(4.731943f, +3f, 1000f);
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
    public List<Sprite> confeteSprites;
    public GameObject confetePrefab;
    public Transform confeteParent;
    public int quantidadeConfetes = 30;

    [Header("Áudios")]
    public List<AudioClip> audiosBoquinhas;
    public List<AudioClip> audiosFiguras;
    public AudioSource audioSource;
    public AudioClip somCortina;
    private AudioClip audioBoquinhaCorreta;

    // Dicionários principais
    private Dictionary<Sprite, AudioClip> mapaBoca = new();
    private Dictionary<Sprite, Sprite> mapaLetra = new();
    private Dictionary<Sprite, AudioClip> mapaFigura = new();

    private void Start()
    {
        coelhoTransform = coelho.GetComponent<Transform>();
        coelhoTransform.position = posicaoInicialCoelho;
        coelhoAnimator = coelho.GetComponent<Animator>();
        boquinhaCorreta = GO_boquinhaCorreta.GetComponent<SpriteRenderer>();
        difficulty = GameObject.FindObjectOfType<DifficultySaveManager>();
        modoAtual = difficulty.GetSavedDifficultyLevel();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "RoundManager_Sample")
            roundManager = GameObject.FindObjectOfType<RoundManager>();

        // Cria os mapas
        CriarMapaBoca();
        CriarMapaLetra();
        CriarMapaFigura();

        PrepararRodada();
    }

    public void CriarMapaBoca()
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

    public void CriarMapaLetra()
    {
        mapaLetra.Clear();
        foreach (var letra in letrasDisponiveis)
        {
            string nomeLetra = letra.name;
            var nomeBoca = nomeLetra; // Nome igual, se desejar mapeamento mais complexo crie um dicionário de string → string

            Sprite boca = boquinhasDisponiveis.Find(b => b.name == nomeBoca);
            if (boca != null)
                mapaLetra[letra] = boca;
            else
                Debug.LogWarning($"Boca '{nomeBoca}' não encontrada para a letra '{nomeLetra}'.");
        }
    }

    public void CriarMapaFigura()
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

    public IEnumerator IniciarGameplay()
    {
        yield return new WaitForSeconds(2f);
        cortinaEsquerdaAnimator?.SetTrigger("abrir");
        cortinaDireitaAnimator?.SetTrigger("abrir");
        audioSource.PlayOneShot(somCortina);

        yield return new WaitForSeconds(2f);
        coelhoAnimator?.SetTrigger("aparecer");

        imageFalaCoelho.SetActive(true);
        GO_boquinhaCorreta.SetActive(true);
        coelhoTransform.position = posicaoFinalCoelho;

        yield return new WaitForSeconds(0.5f);

        PlayAudioBoquinha();

        canhao.podeDisparar = true;
    }

    public void PrepararRodada()
    {
        int quantidade = ObterNumeroDeImagens();

        List<Sprite> fonte = modoAtual switch
        {
            DifficultyLevels.Mouths => boquinhasDisponiveis,
            DifficultyLevels.Letters => letrasDisponiveis,
            DifficultyLevels.Figures => figurasDisponiveis,
            _ => new List<Sprite>()
        };

        List<Sprite> sorteados = SortearAleatorios(fonte, quantidade);

        figuras6GO.SetActive(quantidade == 6);
        figuras8GO.SetActive(quantidade == 8);
        figuras12GO.SetActive(quantidade == 12);

        int idx = Random.Range(0, sorteados.Count);

        AtualizarIconesPainel(sorteados, idx);

        Sprite resposta = sorteados[idx];

        if (modoAtual == DifficultyLevels.Letters && mapaLetra.TryGetValue(resposta, out Sprite boca))
        {
            boquinhaCorreta.sprite = boca;
            if (mapaBoca.TryGetValue(boca, out AudioClip clip))
                audioBoquinhaCorreta = clip;
        }
        else if (modoAtual == DifficultyLevels.Mouths)
        {
            boquinhaCorreta.sprite = resposta;
            if (mapaBoca.TryGetValue(resposta, out AudioClip clip))
                audioBoquinhaCorreta = clip;
        }
        else if (modoAtual == DifficultyLevels.Figures)
        {
            if (mapaFigura.TryGetValue(resposta, out AudioClip clip))
                audioBoquinhaCorreta = clip;
        }
    }

    private void AtualizarIconesPainel(List<Sprite> sprites, int respostaCorretaIndex)
    {
        int total = sprites.Count;
        List<Image> iconesAtuais = total switch
        {
            6 => iconesPainel_6,
            8 => iconesPainel_8,
            12 => iconesPainel_12,
            _ => new List<Image>()
        };

        for (int i = 0; i < iconesAtuais.Count; i++)
        {
            var icone = iconesAtuais[i];

            if (i < total)
            {
                icone.gameObject.SetActive(true);
                icone.sprite = sprites[i];

                var guess = icone.GetComponent<GuessItem>() ?? icone.gameObject.AddComponent<GuessItem>();
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

    private string NormalizarNome(string nome)
    {
        string normalizado = nome.ToLower().Normalize(System.Text.NormalizationForm.FormD);
        System.Text.StringBuilder sb = new();

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
    public void DispararConfetes()
    {
        for (int i = 0; i < quantidadeConfetes; i++)
        {
            Vector3 posicaoSpawn = new Vector3(
                Random.Range(-3.0f, 3.0f),
                5f + Random.Range(0f, 2.5f),
                1000f
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
                float forcaX = Random.Range(-4.0f, 4.0f);
                float forcaY = Random.Range(3f, 8f);
                rb.AddForce(new Vector2(forcaX, forcaY), ForceMode2D.Impulse);

                float torque = Random.Range(-5f, 5f);
                rb.AddTorque(torque, ForceMode2D.Impulse);
            }

            StartCoroutine(DestruirQuandoForaDaTela(confete));
        }
    }

    private IEnumerator DestruirQuandoForaDaTela(GameObject confete)
    {
        yield return new WaitForSeconds(3f);

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

}
