using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Boquinhas.Core;
using UnityEngine.SceneManagement;

public class GameManagerBoquinhas : MonoBehaviour
{

    public DifficultyLevels modoAtual;
    private DifficultySaveManager difficulty;
    private RoundManager roundManager;

    [Header("UI")]
    public GameObject painelFiguras;
    public GameObject painelBocasLetras;
    public List<Image> iconesPainelCentral;

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

    private Dictionary<Sprite, Sprite> mapaLetraParaBoquinha = new();
    Dictionary<string, string> nomeLetraParaBoquinha = new Dictionary<string, string>
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
    {"H", "Z-ASA"}, // ou o que for correto
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
    {"Ó", "Ó"},
    {"Ô", "Ô"}
};


    private void Start()
    {
        coelhoTransform = coelho.GetComponent<Transform>();
        coelhoTransform.position = posicaoInicialCoelho;
        coelhoAnimator = coelho.GetComponent<Animator>();
        boquinhaCorreta = GO_boquinhaCorreta.GetComponent<SpriteRenderer>();
        difficulty = (DifficultySaveManager)GameObject.FindObjectOfType(typeof(DifficultySaveManager));
        modoAtual = difficulty.GetSavedDifficultyLevel();

        if (SceneManager.GetActiveScene().name == "RoundManager_Sample")
        {
            roundManager = (RoundManager)GameObject.FindObjectOfType(typeof(RoundManager));
        }

        CriarMapaLetraBoquinha();
        PrepararRodada();
        // Inicia o fluxo do jogo aqui

    }

    public IEnumerator IniciarGameplay()
    {
        // TODO: Exibir desafio específico conforme modo



        yield return new WaitForSeconds(2.0f);

        cortinaEsquerdaAnimator?.SetTrigger("abrir");
        cortinaDireitaAnimator?.SetTrigger("abrir");

        yield return new WaitForSeconds(2.0f);

        coelhoAnimator?.SetTrigger("aparecer");
        coelhoTransform.position = posicaoFinalCoelho;
        imageFalaCoelho.SetActive(true);
        GO_boquinhaCorreta.SetActive(true);

      
        canhao.podeDisparar = true;
        yield return new WaitForSeconds(1.5f);

    }

    public void PrepararRodada()
    {
        List<Sprite> sorteados;

        switch (modoAtual)
        {
            case DifficultyLevels.Mouths:
                painelBocasLetras.SetActive(true);
                painelFiguras.SetActive(false);
                sorteados = SortearAleatorios(boquinhasDisponiveis, 3);
                AtualizarIconesPainel(sorteados, Random.Range(0, sorteados.Count));
                break;

            case DifficultyLevels.Letters:
                painelBocasLetras.SetActive(true);
                painelFiguras.SetActive(false);
                sorteados = SortearAleatorios(letrasDisponiveis, 3);
                int idx = Random.Range(0, sorteados.Count);
                AtualizarIconesPainel(sorteados, idx);
                if (mapaLetraParaBoquinha.TryGetValue(sorteados[idx], out Sprite boca))
                    boquinhaCorreta.sprite = boca;
                break;

            case DifficultyLevels.Figures:
                painelBocasLetras.SetActive(false);
                painelFiguras.SetActive(true);
                sorteados = SortearAleatorios(figurasDisponiveis, 9);
                AtualizarIconesPainel(sorteados);
                break;
        }
    }
    public void AtualizarIconesPainel(List<Sprite> sprites, int indexCorreto)
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

            if (nomeLetraParaBoquinha.TryGetValue(nomeLetra, out string nomeBoquinha))
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

}
