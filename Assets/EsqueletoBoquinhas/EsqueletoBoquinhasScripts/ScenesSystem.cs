using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesSystem : MonoBehaviour
{
    public SceneTransitionController stc;

    private string _sceneName = "MainMenu";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //print("ENABLED");
    }

    public void ChangeScene(string sceneName)
    {
        _sceneName = sceneName;
        stc = FindObjectOfType<SceneTransitionController>();
        stc.TransitionOut();
    }

    public void Loader()
    {
        //print("Loading scene: " + _sceneName);
        SceneManager.LoadScene(_sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        stc = FindObjectOfType<SceneTransitionController>();
        stc.TransitionIn();
    }
}