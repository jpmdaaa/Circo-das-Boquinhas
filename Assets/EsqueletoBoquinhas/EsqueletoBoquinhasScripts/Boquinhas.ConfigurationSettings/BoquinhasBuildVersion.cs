using UnityEngine;

namespace Boquinhas.ConfigurationSettings
{
    public class BoquinhasBuildVersion : ScriptableObject
    {
        public string version;
        //string version = "v 1.0."+ DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString("00")+DateTime.Now.Hour.ToString("00")+DateTime.Now.Minute.ToString("00")+" ["+EsqueletoVersion.version+"]";
    }
}