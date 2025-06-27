using System.Collections;
using System.Collections.Generic;
using Boquinhas.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Boquinhas.ConfigurationSettings
{
    public class GameEnd_BoquinhasConfigurationSettings : MonoBehaviour
    {
        [Header("Options")] public GameObject btnPlayAgain;

        public GameObject btnNextLevel;

        public GameObject slot0;
        public GameObject slot1;
        public GameObject slot2;
        public GameObject slot3;
        public List<GameObject> confetes;
        public TMP_Text text;
        private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;

        [Header("Áudios")]
        public AudioClip audioShowBravo;
        public AudioClip audioShowFracassou;
        public AudioSource audioSource;

        private void Awake()
        {
            _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
            if (_boquinhasConfigurationSettings.playAgainOnly)
            {
                btnNextLevel.SetActive(false);
                btnPlayAgain.transform.SetPositionAndRotation(
                    new Vector3(0, btnPlayAgain.transform.position.y, btnPlayAgain.transform.position.z),
                    btnPlayAgain.transform.rotation);
            }
        }

        private void Start()
        {
            var holder = FindObjectOfType<PontuacaoHolder>();

            if (holder != null)
            {
                var pontuacoes = holder.pontuacoes;
                int jogadoresValidos = 0;
                int jogadoresBons = 0;

                foreach (var p in pontuacoes)
                {
                    if (p.totalDeRodadas > 0)
                    {
                        jogadoresValidos++;
                        if (p.CalcularPorcentagem() >= 50f)
                            jogadoresBons++;
                    }
                }

                bool sucesso = jogadoresBons >= Mathf.CeilToInt(jogadoresValidos / 2f);

                if (sucesso)
                {
                    ShowFeedbackPositivo();
                }
                else
                {
                    ShowFeedbackNegativo();
                }

                StartCoroutine(IShowStars());
            }
        }


        private void ShowFeedbackPositivo()
        {
            text.text = "BRAVO!! QUE SHOW!!";

            foreach (var confete in confetes)
                confete.SetActive(true);

            if (audioSource != null && audioShowBravo != null)
            {
                audioSource.clip = audioShowBravo;
                audioSource.Play();
            }

            AudioSystem.Instance.PlaySilenceableNarration(audioShowBravo);
        }

        private void ShowFeedbackNegativo()
        {
            text.text = "O SHOW FRACASSOU";

            foreach (var confete in confetes)
                confete.SetActive(false); // ou uma animação triste, se quiser

            if (audioSource != null && audioShowFracassou != null)
            {
                audioSource.clip = audioShowFracassou;
                audioSource.Play();
            }

            AudioSystem.Instance.PlaySilenceableNarration(audioShowFracassou);
        }


        private IEnumerator IShowStars()
        {
            var holder = FindObjectOfType<PontuacaoHolder>();
            yield return new WaitForSeconds(0.5f);

            if (holder != null)
            {
                var pontuacoes = holder.pontuacoes;

                if (slot0.GetComponent<PlayerIcon>().avatar.activeSelf)
                {
                    int estrelas = pontuacoes[0].CalcularEstrelas();
                    slot0.GetComponent<PlayerIcon>().ShowStars(estrelas);
                    yield return new WaitForSeconds(2f);
                }

                if (slot1.GetComponent<PlayerIcon>().avatar.activeSelf)
                {
                    int estrelas = pontuacoes[1].CalcularEstrelas();
                    slot1.GetComponent<PlayerIcon>().ShowStars(estrelas);
                    yield return new WaitForSeconds(2f);
                }

                if (slot2.GetComponent<PlayerIcon>().avatar.activeSelf)
                {
                    int estrelas = pontuacoes[2].CalcularEstrelas();
                    slot2.GetComponent<PlayerIcon>().ShowStars(estrelas);
                    yield return new WaitForSeconds(2f);
                }

                if (slot3.GetComponent<PlayerIcon>().avatar.activeSelf)
                {
                    int estrelas = pontuacoes[3].CalcularEstrelas();
                    slot3.GetComponent<PlayerIcon>().ShowStars(estrelas);
                    yield return new WaitForSeconds(2f);
                }

                Destroy(holder.gameObject); // limpa da memória
            }
        }

    }
}