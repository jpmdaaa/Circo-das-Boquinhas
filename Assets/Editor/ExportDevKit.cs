using System.Collections.Generic;
using System.IO;
using System.Linq;
using Playmove.Core.Editor;
using UnityEditor;
using UnityEngine;

public class ExportDevKit : Editor
{
    [MenuItem("Playmove/Export DevKit", priority = 0)]
    private static void Export()
    {
        var savePath = PlaytablePrefs.Get("DevKitExportPath", string.Empty);
        savePath = EditorUtility.OpenFolderPanel("Select folder to export DevKit", savePath, string.Empty);
        if (string.IsNullOrEmpty(savePath)) return;
        PlaytablePrefs.Set("DevKitExportPath", savePath);

        // Package Parceiros
        DevKit.Version.Value++;
        EditorUtility.SetDirty(DevKit.Version);
        AssetDatabase.SaveAssets();

        var allAssetsPath = new List<string>(GetCoreAssets());
        allAssetsPath.AddRange(GetAvatarAssets());
        allAssetsPath.AddRange(GetMetricsAssets());
        allAssetsPath.AddRange(GetFrameworkAssets());
        ExportPackage(allAssetsPath.ToArray(), savePath, "DevKit_New");

        // Package Parceiros + Management
        allAssetsPath.AddRange(GetManagementAssets());
        ExportPackage(allAssetsPath.ToArray(), savePath, "DevKit+Management_New");

        // Package Parceiros Update
        allAssetsPath.Clear();
        allAssetsPath = new List<string>(GetCoreAssets(false));
        allAssetsPath.AddRange(GetAvatarAssets(false));
        allAssetsPath.AddRange(GetMetricsAssets(false));
        allAssetsPath.AddRange(GetFrameworkAssets());
        ExportPackage(allAssetsPath.ToArray(), savePath, "DevKit_Update");

        // Package Jogos Playmove Update
        allAssetsPath.Clear();
        allAssetsPath = new List<string>(GetCoreAssets(false));
        allAssetsPath.AddRange(GetAvatarAssets(false));
        allAssetsPath.AddRange(GetMetricsAssets(false));
        allAssetsPath.AddRange(GetFrameworkAssets(true));
        ExportPackage(allAssetsPath.ToArray(), savePath, "DevKit_Playmove_Update");

        // Package Parceiros + Management Update
        allAssetsPath.AddRange(GetManagementAssets());
        ExportPackage(allAssetsPath.ToArray(), savePath, "DevKit+Management_Update");
    }

    private static string[] GetCoreAssets(bool newPackage = true)
    {
        var assetsPath = new List<string>(GetAssetsPath("/Playmove/Core"));
        assetsPath.AddRange(GetAssetsPath("/ThirdParty"));
        assetsPath.AddRange(GetAssetsPath("/Bundles/Global/Localization"));
        if (newPackage)
        {
            assetsPath.AddRange(GetAssetsPath("/AssetsCatalog"));
        }
        else
        {
            // RemoveAsset(assetsPath, "/Core/Scenes/PlaytableBootstrap.unity");
            RemoveAsset(assetsPath, "/Scripts/Bundles/AssetsCatalogs/AssetsCatalog.cs");
            RemoveAsset(assetsPath, "/Resources/GameSettings.asset");
            RemoveAsset(assetsPath, "/Resources/PlaytableAudioMixer.mixer");
            RemoveAsset(assetsPath, "/Editor/Datas/BuildVersion.asset");
        }

        return assetsPath.ToArray();
    }

    private static string[] GetAvatarAssets(bool newPackage = true)
    {
        return GetAssetsPath("/Playmove/Avatar");
    }

    private static string[] GetMetricsAssets(bool newPackage = true)
    {
        return GetAssetsPath("/Playmove/Metrics");
    }

    private static string[] GetManagementAssets(bool newPackage = true)
    {
        return GetAssetsPath("/Playmove/Management");
    }

    private static string[] GetFrameworkAssets(bool playmove = false)
    {
        var assetsPath = new List<string>(GetAssetsPath("/Playmove/Framework"));
        assetsPath.AddRange(GetAssetsPath("/Bundles/Global/Data/PlaymoveFramework"));
        assetsPath.AddRange(GetAssetsPath("/Bundles/Global/Localization"));
        if (!playmove)
            RemoveAsset(assetsPath, "/Playmove/Framework/Scripts/Localizers/LocalizerPYText.cs");

        return assetsPath.ToArray();
    }

    private static void RemoveAsset(List<string> assets, string assetPath)
    {
        foreach (var asset in assets.GetRange(0, assets.Count))
            if (asset.Contains(assetPath))
                assets.Remove(asset);
    }

    private static string[] GetAssetsPath(string rootFolder)
    {
        return Directory.GetFiles(Application.dataPath + rootFolder, "*.*", SearchOption.AllDirectories)
            .Select(path => path.Replace(Application.dataPath, "Assets").Replace(@"\", "/"))
            .ToArray();
    }

    private static void ExportPackage(string[] assetsPath, string savePath, string packageName)
    {
        AssetDatabase.ExportPackage(assetsPath, $"{savePath}/{packageName}.unitypackage",
            ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
    }
}