using System.Collections.Generic;
using System.IO;
using System.Linq;
using Playmove.Core.Bundles;
using UnityEditor;
using UnityEngine;

namespace Playmove.Core.Editor.Bundles
{
    public class DetectChangesInBundles : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets
                         .Where(path => path.Contains("BundlesAssets") || path.Contains("Bundles"))
                         .Select(FormatAssetPathToBundlePath))
                SetBundleDirty(path, true);
            foreach (var path in deletedAssets.Where(path => path.Contains("BundlesAssets") || path.Contains("Bundles"))
                         .Select(FormatAssetPathToBundlePath))
                SetBundleDirty(path, true);
            foreach (var path in movedAssets.Where(path => path.Contains("BundlesAssets") || path.Contains("Bundles"))
                         .Select(FormatAssetPathToBundlePath))
                SetBundleDirty(path, true);
            foreach (var path in movedFromAssetPaths
                         .Where(path => path.Contains("BundlesAssets") || path.Contains("Bundles"))
                         .Select(FormatAssetPathToBundlePath))
                SetBundleDirty(path, true);

            // Verify bundles exist
            var paths = GetAllBundlesPath();
            for (var i = 0; i < paths.Count; i++)
                if (!File.Exists(Application.dataPath + PlaytablePrefs.Get(SimplifyPathKey(paths[i]), string.Empty)))
                    SetBundleDirty(paths[i], true);
        }

        public static List<string> GetBundlesPathDirty()
        {
            var bundlesPath = new List<string>();
            foreach (var pathKey in GetAllBundlesPath())
                if (GetBundleDirty(pathKey))
                    bundlesPath.Add(pathKey);
            return bundlesPath;
        }

        public static bool GetBundleDirty(string pathKey)
        {
            pathKey = pathKey.Replace(@"\", "/");
            return string.IsNullOrEmpty(PlaytablePrefs.Get(SimplifyPathKey(pathKey), string.Empty))
                   || !File.Exists(Application.dataPath + PlaytablePrefs.Get<string>(SimplifyPathKey(pathKey)));
        }

        public static void SetBundleDirty(string pathKey, bool dirty)
        {
            if (!pathKey.Contains("/BundlesAssets/") && !pathKey.Contains("/Bundles/"))
                return;

            var dirKeyInfo = new DirectoryInfo(pathKey);
            var globalOrExpansion = pathKey.Contains("/Global/") ? "global" : "expansions";
            var bundlePath =
                $"{PlaytableBundlesPath.GetBuildDirectory(pathKey)}/{globalOrExpansion}_{dirKeyInfo.Name.ToLower()}.bundle";
            PlaytablePrefs.Set(SimplifyPathKey(pathKey),
                dirty ? string.Empty : bundlePath.Replace(Application.dataPath, string.Empty));
        }

        private static List<string> GetAllBundlesPath()
        {
            var paths = new List<string>(PlaytableBundlesPath.GetContentsPath());
            paths.AddRange(PlaytableBundlesPath.GetDatasPath());
            paths.AddRange(PlaytableBundlesPath.GetLocalizationsPath());

            foreach (var expansionName in PlaytableBundlesPath.GetExpansionNames())
            {
                paths.AddRange(PlaytableBundlesPath.GetContentsPath(expansionName));
                paths.AddRange(PlaytableBundlesPath.GetDatasPath(expansionName));
                paths.AddRange(PlaytableBundlesPath.GetLocalizationsPath(expansionName));
            }

            return paths;
        }

        private static string FormatAssetPathToBundlePath(string assetPath)
        {
            var path = new FileInfo(assetPath).Directory.FullName.Replace(@"\", "/");
            var array = new List<string>(path.Split('/'));
            var indexOf = Mathf.Max(array.IndexOf("Content"), array.IndexOf("Data"), array.IndexOf("Localization"));
            if (indexOf + 2 > array.Count) return string.Empty;
            return string.Join("/", array.ToArray(), 0, indexOf + 2);
        }

        private static string SimplifyPathKey(string fullPathKey)
        {
            return fullPathKey.Replace(Application.dataPath, $"{DevKit.ProjectName}/Assets");
        }
    }
}