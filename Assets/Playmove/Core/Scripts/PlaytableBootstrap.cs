using Playmove.Metrics.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Playmove.Core
{
    public class PlaytableBootstrap : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoad = string.Empty;

        private void Awake()
        {
            TestConnection();
        }

        private void TestConnection()
        {
            Playtable.Instance.OnPlaytableReady.AddListener(LoadNextScene);
            if (Playtable.Instance.Key == null)
            {
                Invoke("TestConnection", .02f);
                Playtable.Instance.OnPlaytableReady.RemoveListener(LoadNextScene);
                return;
            }

            Playtable.Instance.Initialize();
        }

        private void LoadNextScene()
        {
            Playtable.Instance.OnPlaytableReady.RemoveListener(LoadNextScene);

            //Used to Start Session
            MetricsAPI.StartSession();
            // ---
            if (string.IsNullOrEmpty(_sceneToLoad))
                SceneManager.LoadScene(1);
            else
                SceneManager.LoadScene(_sceneToLoad);
        }
    }
}