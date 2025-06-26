using System.Collections;
using Boquinhas.Core;
using UnityEngine;

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

        private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;

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
            //show the stars for each player
            StartCoroutine(IShowStars());
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