using Boquinhas.ConfigurationSettings;
using UnityEngine;

namespace Boquinhas.Core
{
    [DefaultExecutionOrder(-500)]
    public class GameManager : MonoBehaviour
    {
        public BoquinhasConfigurationSettings boquinhasConfigurationSettings;

        #region Awake

        private void Awake()
        {
            _instance = this;
            SetDifficultyLevel(DifficultyLevels.None);
        }

        #endregion

        public bool CanMistakeOnly2Times()
        {
            return boquinhasConfigurationSettings.skipTurnAfter2Errors;
        }


        #region DifficultyModeSchema

        public DifficultyModeScheme GetDifficultyMode()
        {
            return boquinhasConfigurationSettings.difficultyModeScheme;
        }

        #endregion

        #region Instance

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance is null) Debug.LogError("NULL Game Manager");

                return _instance;
            }
        }

        #endregion

        #region DifficultyLevels

        private static DifficultyLevels _currentDifficultyLevel = DifficultyLevels.None;

        public DifficultyLevels GetCurrentDifficultyLevel()
        {
            return _currentDifficultyLevel;
        }

        public void SetDifficultyLevel(DifficultyLevels difficultyLevel)
        {
            //print(difficultyLevel);
            _currentDifficultyLevel = difficultyLevel;
        }

        #endregion
    }
}