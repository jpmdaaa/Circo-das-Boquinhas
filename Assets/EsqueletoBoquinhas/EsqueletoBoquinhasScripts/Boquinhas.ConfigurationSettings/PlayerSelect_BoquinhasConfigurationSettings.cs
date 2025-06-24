using Boquinhas.Core;
using UnityEngine;

namespace Boquinhas.ConfigurationSettings
{
    public class PlayerSelect_BoquinhasConfigurationSettings : MonoBehaviour
    {
        [Header("Difficulties")] public GameObject easyMediumHard;

        public GameObject easyHard;
        public GameObject mouthsLettersDifficulty;
        public GameObject letterGroup;
        public GameObject letterGroupDifficulty;
        public GameObject mouthLetters;
        public GameObject imagesMouthsLetters;

        private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;
        
        private void Awake()
        {
            _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
            easyMediumHard.SetActive(false);
            easyHard.SetActive(false);
            mouthsLettersDifficulty.SetActive(false);
            letterGroup.SetActive(false);
            letterGroupDifficulty.SetActive(false);
            mouthLetters.SetActive(false);
            imagesMouthsLetters.SetActive(false);
            switch (_boquinhasConfigurationSettings.difficultyModeScheme)
            {
                case DifficultyModeScheme.EasyMediumHard:
                    easyMediumHard.SetActive(true);
                    break;
                case DifficultyModeScheme.EasyHard:
                    easyHard.SetActive(true);
                    break;
                case DifficultyModeScheme.MouthsLettersDifficulty:
                    mouthsLettersDifficulty.SetActive(true);
                    break;
                case DifficultyModeScheme.LetterGroup:
                    letterGroup.SetActive(true);
                    break;
                case DifficultyModeScheme.LetterGroupDifficulty:
                    letterGroupDifficulty.SetActive(true);
                    break;
                case DifficultyModeScheme.MouthLetters:
                    mouthLetters.SetActive(true);
                    break;
                case DifficultyModeScheme.ImageMouthLetters:
                    imagesMouthsLetters.SetActive(true);
                    break;
            }
        }

        public void ChangeScene()
        {
            _boquinhasConfigurationSettings.tutorialSceneName = "Tutorial";
            // Verifica se há tutorial antes de ir para o jogo
            if (!string.IsNullOrEmpty(_boquinhasConfigurationSettings.tutorialSceneName))
            {
                GetComponent<SceneChanger>().ChangeScene(_boquinhasConfigurationSettings.tutorialSceneName);
            }
            else
            {
                GetComponent<SceneChanger>().ChangeScene(_boquinhasConfigurationSettings.gameSceneName);
            }
        }
    }
}
