using UnityEditor;
using UnityEngine;

namespace Boquinhas
{
    public class EsqueletoVersionEditor
    {
        [MenuItem("Boquinhas/Esqueleto/Version v" + EsqueletoVersion.version)]
        public static void VersionCheck()
        {
            Debug.Log("Versão instalada do Esqueleto: v" + EsqueletoVersion.version + " | " + "Release: " +
                      EsqueletoVersion.release);
        }
    }
}