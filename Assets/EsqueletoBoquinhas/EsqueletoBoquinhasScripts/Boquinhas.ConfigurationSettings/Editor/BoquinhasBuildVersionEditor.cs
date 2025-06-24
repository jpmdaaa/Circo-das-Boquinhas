namespace Boquinhas.ConfigurationSettings
{
    public class BoquinhasBuildVersionEditor
    {
        // [MenuItem("Boquinhas/Esqueleto/Create Build Version File")]
        // public static void CreateBuildVersionFile()
        // {
        //
        //     BoquinhasBuildVersion buildVersion = ScriptableObject.CreateInstance<BoquinhasBuildVersion>();
        //     Directory.CreateDirectory("Assets/Data/");
        //     string fileName = "Assets/Data/BoquinhasBuildVersion.asset";
        //     int fileSequential = 0;
        //     while (File.Exists(fileName))
        //     {
        //         fileSequential++;
        //         fileName = "Assets/Data/BoquinhasBuildVersion_" + fileSequential.ToString() + ".asset";
        //     }
        //
        //     AssetDatabase.CreateAsset(buildVersion, fileName);
        //     AssetDatabase.SaveAssets();
        // }

        // [MenuItem("Boquinhas/Esqueleto/Checkversion")]
        // public static void VersionCheck()
        // {
        //     string version = "v 1.0."+ DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString("00")+DateTime.Now.Hour.ToString("00")+DateTime.Now.Minute.ToString("00")+" ["+EsqueletoVersion.version+"]";
        //     Debug.Log(version);
        // }
    }
}