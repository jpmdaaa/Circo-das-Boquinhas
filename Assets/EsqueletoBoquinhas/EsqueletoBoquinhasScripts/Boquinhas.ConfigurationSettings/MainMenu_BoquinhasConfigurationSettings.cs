using Boquinhas.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Boquinhas.ConfigurationSettings
{
    public class MainMenu_BoquinhasConfigurationSettings : MonoBehaviour
    {
        [Header("Logo")] public Image logo;

        private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;


        private void Awake()
        {
            _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
            //logo.sprite = _boquinhasConfigurationSettings.mainMenuLogo;
        }

        private void Start()
        {
            AudioSystem.Instance.PlaySilenceableNarration(_boquinhasConfigurationSettings.gameNameAudio);
        }
    }
}