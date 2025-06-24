using System;
using System.Text.RegularExpressions;
using Playmove.Avatars.API.Models;
using Playmove.Core.BasicEvents;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#else
using Playmove.Core.Bundles;
#endif

namespace Playmove.Core
{
    /// <summary>
    ///     Unity scriptable object to hold informations that playtable needs
    ///     for this game to work
    /// </summary>
    [ScriptOrder(-200)]
    public class GameSettings : ScriptableObject
    {
        private static GameSettings _instance;

        public static int InactiveTime = 0;
        public static PlaytableEventString OnLanguageChanged = new();
        public static PlaytableEventBool OnMuteChanged = new();
        public static PlaytableEventFloat OnVolumeChanged = new();
        public static PlaytableEventFloat OnMicVolumeChanged = new();

        private static SlotsConfig _slotConfigParams;

        // Playmove Settings
        [Tooltip("GUID that Playmove sent")] [SerializeField]
        private string _GUID = "-";

        [Tooltip("Executable Name without .exe, accents and spaces")] [SerializeField]
        private string _executableName = string.Empty;

        // Playtable Settings
        [SerializeField] private string _expansion = string.Empty;
        [SerializeField] private string _language = "pt-BR";

        [SerializeField] private bool _playFromPlaytableBootstrap;

        [SerializeField] private bool _mute;
        [SerializeField] [Range(0, 1)] private float _volume = 0.5f;

        [SerializeField] [Range(0, 1)] private float _micVolume = 0.5f;

        // Avatar Settings
        [SerializeField] [Range(1, 4)] private int _minSlots = 1;
        [SerializeField] [Range(1, 4)] private int _totalSlots = 1;
        [SerializeField] [Range(1, 5)] private int _maxPlayersPerSlot = 5;
        [SerializeField] private bool _hasAI;

        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    _instance = CreateOrFindGameSettings();
#else
                    _instance = Data.GetAsset<GameSettings>("GameSettings");
#endif
                }

                return _instance;
            }
        }

        public static long ApplicationId { get; set; }

        public static string GUID
        {
            get => Instance._GUID;
            set => Instance._GUID = value;
        }

        public static string ExecutableName
        {
            get => Instance._executableName;
            set => Instance._executableName = value;
        }

        public static string Expansion => Instance._expansion;

        public static string Language
        {
            get => Instance._language;
            set
            {
                if (!Regex.IsMatch(value, "[a-z]+-[A-Z]+") && value.Length != 5)
                {
                    Debug.LogWarning("Language is in bad format it should be as follow pt-BR not pt-br!");
                    return;
                }

                Instance._language = value;
                OnLanguageChanged.Invoke(Instance._language);
            }
        }

        public static bool PlayFromPlaytableBootstrap
        {
            get => Instance._playFromPlaytableBootstrap;
            set => Instance._playFromPlaytableBootstrap = value;
        }

        public static bool Mute
        {
            get => Instance._mute;
            set
            {
                Instance._mute = value;
                OnMuteChanged.Invoke(Instance._mute);
            }
        }

        public static float Volume
        {
            get => Instance._volume;
            set
            {
                Instance._volume = Mathf.Clamp01(value);
                OnVolumeChanged.Invoke(Instance._volume);
            }
        }

        public static float MicVolume
        {
            get => Instance._micVolume;
            set
            {
                Instance._micVolume = Mathf.Clamp01(value);
                OnMicVolumeChanged.Invoke(Instance._micVolume);
            }
        }

        public static int MinSlots => Mathf.Clamp(Instance._minSlots, 1, TotalSlots);
        public static int TotalSlots => Instance._totalSlots;
        public static int MaxPlayersPerSlot => Instance._maxPlayersPerSlot;
        public static bool HasAI => Instance._hasAI;

        public static SlotsConfig SlotsConfig
        {
            get
            {
                if (_slotConfigParams == null)
                    _slotConfigParams = new SlotsConfig(TotalSlots, MaxPlayersPerSlot, MinSlots, HasAI);
                return _slotConfigParams;
            }
            set => _slotConfigParams = value;
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public static string FormatData(DateTime date)
        {
            return Language == "en-US" ? date.ToString("MM/dd/y hh:mm tt") : date.ToString("dd/MM/y HH:mm");
        }

        /// <summary>
        ///     Used only for Playtable.cs to set the initial language for the game and it dont need to raise the change event
        /// </summary>
        public static void SetLanguageWithoutRaisingEvent(string language)
        {
            Instance._language = language;
        }

#if UNITY_EDITOR
        public static GameSettings CreateOrFindGameSettings()
        {
            var settings = GetScriptableAsset<GameSettings>("GameSettings", "GameSettings");

            // Create a gameSettings file if any exist in project
            if (settings == null)
            {
                settings = CreateInstance<GameSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/Playmove/Core/Resources/GameSettings.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Selection.activeObject = settings;
                Debug.Log("File GameSettings created at Assets/Playmove/Core/Resources/GameSettings.asset!");
            }

            return settings;
        }

        private static T GetScriptableAsset<T>(string assetName, string assetType)
            where T : ScriptableObject
        {
            T asset = default;
            var gameSettingsGUIDs = AssetDatabase.FindAssets($"{assetName} t:{assetType}");
            if (gameSettingsGUIDs.Length > 0)
                asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(gameSettingsGUIDs[0]));
            return asset;
        }
#endif
    }
}