using Playmove.Avatars.API;
using UnityEngine;

namespace Boquinhas.ConfigurationSettings
{
    public class BoquinhasConfigurationSettings : ScriptableObject
    {
        public string gameName;
        public Sprite mainMenuLogo;
        public Sprite bookSprite;
        public AudioClip gameNameAudio;
        public AudioClip mainMenuMusic;
        public AudioClip gamePlayMusic;
        [Range(0, 1)] public float musicVolume;
        public string howToPlay;
        public DifficultyModeScheme difficultyModeScheme;
        public string gameSceneName;
        public bool playAgainOnly;
        public string tutorialSceneName;

        [Header("Round Options")] public int maxRounds_1Player = 1;

        public int maxRounds_2Players = 1;
        public int maxRounds_3Players = 1;
        public int maxRounds_4Players = 1;
        public float gameDurationEasy = 20;
        public float gameDurationMedium = 15;
        public float gameDurationHard = 15;
        public bool canTryAgain = true;

        [Header("Error Options")] public bool skipTurnAfter2Errors = true;

        public int GetMaxRounds()
        {
            switch (AvatarAPI.SlotsPlaying.Count)
            {
                case 1:
                    return maxRounds_1Player;
                case 2:
                    return maxRounds_2Players;
                case 3:
                    return maxRounds_3Players;
                case 4:
                    return maxRounds_4Players;
            }

            return 0;
        }
    }

    public enum DifficultyModeScheme
    {
        EasyMediumHard,
        EasyHard,
        MouthLetters,
        MouthsLettersDifficulty,
        LetterGroup,
        LetterGroupDifficulty,
        ImageMouthLetters
    }
}