using Boquinhas.Core;
using UnityEngine;

namespace Boquinhas.ConfigurationSettings
{
    public class Settings_BoquinhasConfigurationSettings : MonoBehaviour
    {
        private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;

        private void Awake()
        {
            _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;
            //TODO: Carregar as informações do Boquinhas Configuration Settings para preencher as informações customizadas da cena settings
        }
    }
}