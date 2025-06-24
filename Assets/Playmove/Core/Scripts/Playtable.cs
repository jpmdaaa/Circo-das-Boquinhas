using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Playmove.Avatars;
using Playmove.Core.API;
using Playmove.Core.API.Vms;
using Playmove.Core.Audios;
using Playmove.Core.BasicEvents;
using Playmove.Core.Bundles;
using Playmove.Core.Controls;
using Playmove.Framework;
using Playmove.Metrics.API;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Playmove.Core
{
    /// <summary>
    ///     Main core class to work with playtable kit from here you can access the APIs,
    ///     Current Language, Expansion, Volume, Mute, etc...
    /// </summary>
    [ScriptOrder(-190)]
    public class Playtable : MonoBehaviour
    {
        private static Playtable _instance;

        public static string LeagueGUID = "9364C9E3-02E4-4C30-94F1-E384ADC7B129";

        [SerializeField] private GameObject _waitAvatarToCloseObj;

        /// <summary>
        ///     Event raised when game gets validated by SOP
        /// </summary>
        public PlaytableEvent OnPlaytableReady = new();

#if UNITY_EDITOR
        [SerializeField] private bool _authInEditor;
#endif

        private Authenticate _authentication;

        private float _micVolume;

        public static Playtable Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<Playtable>();
                return _instance;
            }
            private set => _instance = value;
        }

        public GameObject WaitAvatarToCloseObj => _waitAvatarToCloseObj;

        private bool AuthInSOP
        {
            get
            {
#if UNITY_EDITOR
                return _authInEditor;
#else
                return !Debug.isDebugBuild;
#endif
            }
        }

        public bool IsReady => IsValidated && PlaytableBundle.Instance.IsLoaded && AvatarNotification.ChekedSlots;
        public bool IsValidated { get; private set; }
        public string Key { get; private set; }

        [HideInInspector]public string APIBaseURL = "http://localhost:8081/Api";
        public bool useAltDebugURL = false;
        public string altDebugURL = "";
        
        public string APIVersions { get; } = "/v2";

        private Authenticate Authentication
        {
            get
            {
                if (_authentication == null)
                    _authentication = new Authenticate();
                return _authentication;
            }
        }

        private void Awake()
        {
            #if UNITY_EDITOR
            if (useAltDebugURL)
            {
                APIBaseURL = altDebugURL;
                Debug.LogError("WE ARE USING THE ALTERNATIVE URL: " + APIBaseURL + "REMEMBER TO UNCHECK THE BOX BEFORE COMMITING, OTHERWISE THE KOALA WILL BE MAD!");
            }
            #endif
            
            // We have more than one Playtable in scene, destroy the newer one
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            // Check for DLL
            try
            {
                if (!Authentication.IsValid())
                    Debug.LogWarning("Dll not authenticated. Use only on SopVirtual!");
            }
            catch
            {
                ForceExit(888, "Authentication DLL is missing!");
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            PlaytableBundle.Instance.Load(() =>
            {
                var arguments = ReadSopArguments();
                Key = arguments.Key;

                if (!string.IsNullOrEmpty(arguments.Language) && GameSettings.Language != arguments.Language)
                {
                    GameSettings.Language = arguments.Language;
                    Localization.Reload(null);
                }

                GameSettings.OnLanguageChanged.AddListener(OnLanguageChanged);

                if (!IsSopKeyValid(Key)) ForceExit(900, "Key passed by game to SOP is not valid!");
            });
        }

        private void OnApplicationQuit()
        {
            MetricsAPI.EndSession();
        }

        public void Initialize()
        {
            SyncVolumeMute();
            SyncMicVolumeMute();
            // ---
            var inactivity = GetComponent<Inactivity>();
            if (inactivity != null)
                inactivity.GetInactiveTime();
            // ---
            AuthGame();
        }

        public void ReloadGameFromBootstrap()
        {
            AudioManager.StopChannel(AudioChannel.Master, 0.5f);
            AudioManager.StopChannel(AudioChannel.Music, 0.5f);
            AudioManager.StopChannel(AudioChannel.SFX, 0.5f);
            AudioManager.StopChannel(AudioChannel.Voice, 0.5f);
            Localization.Bundles = null;
            PlaytableBundle.Instance.Reload();
            if (ControlCenter.Instance != null)
                ControlCenter.Instance.Close();

            Fader.FadeTo(1, 0.75f, () =>
            {
                PlaytableBundle.Instance.Reload(() =>
                {
                    Fader.FadeTo(0, 0.5f);
                    SceneManager.LoadScene(0);
                });
            });
        }

        /// <summary>
        ///     Read arguments that SOP sends to game
        /// </summary>
        /// <returns></returns>
        private SopArguments ReadSopArguments()
        {
            var sopParamsExe = Environment.GetCommandLineArgs();
            var arguments = new SopArguments
            {
#if DEBUG
                Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(GameSettings.GUID)) + "#123",
                Language = GameSettings.Language
#endif
            };

            foreach (var param in sopParamsExe)
                if (param.StartsWith("key:"))
                {
                    arguments.Key = param.Replace("key:", string.Empty);
                }
                else if (param.StartsWith("lang:"))
                {
                    var language = param.Replace("lang:", string.Empty);
                    if (ProjectContainsLanguage(language))
                        arguments.Language = language;
                }
                else if (param.StartsWith("expan:"))
                {
                    // TODO: Verificar como o sop está mandando o nome das expansões
                }

            return arguments;
        }

        /// <summary>
        ///     Check if the game contains the current language
        /// </summary>
        /// <param name="language">Language to verify</param>
        /// <returns></returns>
        private bool ProjectContainsLanguage(string language)
        {
            return !string.IsNullOrEmpty(PlaytableBundlesPath.GetLocalizationsPath()
                .Find(lang => lang.ToLower().EndsWith(language.ToLower())));
        }

        /// <summary>
        ///     Verify if the game KEY is valid
        /// </summary>
        /// <param name="key">Game KEY</param>
        /// <returns></returns>
        private bool IsSopKeyValid(string key)
        {
            if (AuthInSOP)
            {
                if (string.IsNullOrEmpty(key))
                    return false;

                var keySplit = key.Split('#');
                var finalKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(GameSettings.GUID));
                return finalKey == keySplit[0];
            }

            return true;
        }

        /// <summary>
        ///     Close game with a step and a reason
        /// </summary>
        /// <param name="step">Step where game was closed</param>
        /// <param name="reason">Reason why the game was closed</param>
        public void ForceExit(int step, string reason)
        {
            MetricsAPI.EndSession(result =>
            {
                if (result.Success)
                {
                    Debug.LogError($"Playtable Launcher Error: Step: {step} | Reason: {reason}");
                    if (!Application.isEditor)
                        Process.GetCurrentProcess().Kill();
                }
            });
        }

        /// <summary>
        ///     Close game with a reason
        /// </summary>
        /// <param name="reasong">Reason why the game was closed</param>
        public void ForceExit(string reason = "")
        {
            MetricsAPI.EndSession(result =>
            {
                if (result.Success)
                {
                    Debug.LogError($"Playtable Close Reason: {reason}");
                    if (!Application.isEditor)
                        Process.GetCurrentProcess().Kill();
                }
            });
        }

        /// <summary>
        ///     Gets volume from SOP and sync
        /// </summary>
        private void SyncVolumeMute()
        {
            PlaytableAPI.GetVolume(result =>
            {
                GameSettings.OnVolumeChanged.AddListener(OnVolumeChanged);

                if (!result.HasError)
                    GameSettings.Volume = result.Data;
            });
            PlaytableAPI.GetMute(result =>
            {
                GameSettings.OnMuteChanged.AddListener(OnMuteChanged);

                if (!result.HasError)
                    GameSettings.Mute = result.Data;
            });
        }

        /// <summary>
        ///     Gets Mic volume from SOP and sync
        /// </summary>
        private void SyncMicVolumeMute()
        {
            PlaytableAPI.GetMicrophoneSensitivity(result =>
            {
                GameSettings.OnMicVolumeChanged.AddListener(OnMinVolumeChanged);

                if (!result.HasError)
                    GameSettings.MicVolume = result.Data;
            });
        }

        /// <summary>
        ///     Authenticate game in SOP
        /// </summary>
        private void AuthGame()
        {
            PlaytableAPI.Application.Get(GameSettings.GUID, appResult =>
            {
                if (AuthInSOP)
                {
                    if (appResult.HasError)
                    {
                        ForceExit(800, $"Could not get application info from SOP. Error: {appResult.Error}" +
                                       $" | Check if the game GUID({GameSettings.GUID}) is registered on database");
                        return;
                    }

                    GameSettings.ApplicationId = appResult.Data.Id;
                    PlaytableAPI.Authenticate(GameSettings.ExecutableName, authResult =>
                    {
                        if (Authentication.IsValid())
                        {
                            if (authResult.HasError)
                            {
                                ForceExit(999, authResult.Error);
                            }
                            else
                            {
                                var step = Authentication.SopAuthentication(authResult.Data);
                                if (step != 0)
                                {
                                    ForceExit(step, "Could not Authenticate!");
                                }
                                else
                                {
                                    IsValidated = true;
                                    OnPlaytableReady.Invoke();
                                }
                            }
                        }
                    });
                }
                else
                {
                    if (!appResult.HasError)
                        GameSettings.ApplicationId = appResult.Data.Id;

                    IsValidated = true;
                    OnPlaytableReady.Invoke();
                }
            });
        }

        private void OnLanguageChanged(string language)
        {
            Fader.FadeTo(1, .5f,
                () =>
                {
                    PlaytableAPI.Prefs.Set("localizacao", ValorTipo.String, language,
                        result => { ReloadGameFromBootstrap(); });
                });
        }

        private void OnVolumeChanged(float volume)
        {
            AudioListener.volume = volume;

            if (IsInvoking("changeDBVolume"))
                CancelInvoke("changeDBVolume");
            Invoke("changeDBVolume", .5f);
        }

        private void changeDBVolume()
        {
            PlaytableAPI.SetVolume(AudioListener.volume);
        }

        private void OnMuteChanged(bool mute)
        {
            AudioListener.volume = mute ? 0 : GameSettings.Volume;
            PlaytableAPI.SetMute(mute);
        }

        private void OnMinVolumeChanged(float volume)
        {
            _micVolume = volume;

            if (IsInvoking("changeMicDBVolume"))
                CancelInvoke("changeMicDBVolume");
            Invoke("changeMicDBVolume", .5f);
        }

        private void changeMicDBVolume()
        {
            PlaytableAPI.SetMicrophoneSensitivity(_micVolume);
        }

        public class SopArguments
        {
            public List<string> ExpansionsGUID;
            public string Key;
            public string Language;

            public SopArguments()
            {
                Key = string.Empty;
                Language = string.Empty;
                ExpansionsGUID = new List<string>();
            }
        }
    }
}