using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExportEsqueleto : MonoBehaviour
{
    [MenuItem("Boquinhas/Esqueleto/Export Updated Version")]
    private static void Export()
    {
        //TODO: Remover DevKit GameSettings do .unitypackage
        var exportedPackageAssetList = new List<string>();
        exportedPackageAssetList.Add("Assets/EsqueletoBoquinhas");
        //Export Esqueleto and dependencies into a .unitypackage
        AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(),
            "Esqueleto_v" + EsqueletoVersion.version + ".unitypackage",
            ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Esqueleto Version v" + EsqueletoVersion.version + " Exported on project root folder!");
    }
}