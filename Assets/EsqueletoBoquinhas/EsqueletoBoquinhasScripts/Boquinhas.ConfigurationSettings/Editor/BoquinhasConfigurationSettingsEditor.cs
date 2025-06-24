using System.IO;
using UnityEditor;
using UnityEngine;

namespace Boquinhas.ConfigurationSettings
{
    public class BoquinhasConfigurationSettingsEditor
    {
        [MenuItem("Boquinhas/Esqueleto/Create Configuration Settings")]
        public static void CreateConfigurationSettings()
        {
            var wordData = ScriptableObject.CreateInstance<BoquinhasConfigurationSettings>();
            Directory.CreateDirectory("Assets/Data/");
            var fileName = "Assets/Data/GameBoquinhasConfigurationSettings.asset";
            var fileSequential = 0;
            while (File.Exists(fileName))
            {
                fileSequential++;
                fileName = "Assets/Data/GameBoquinhasConfigurationSettings_" + fileSequential + ".asset";
            }

            AssetDatabase.CreateAsset(wordData, fileName);
            AssetDatabase.SaveAssets();
        }
    }
}