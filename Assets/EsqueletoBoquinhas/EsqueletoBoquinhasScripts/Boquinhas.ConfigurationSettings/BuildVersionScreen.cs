using Boquinhas.ConfigurationSettings;
using TMPro;
using UnityEngine;

public class BuildVersionScreen : MonoBehaviour
{
    public BoquinhasBuildVersion buildVersion;
    public TextMeshProUGUI txtVersion;

    private void Start()
    {
        txtVersion.text = buildVersion.version;
    }
}