using System.Collections;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public AudioClip sound;
    //!!!!!
    //This script is only really usefull for calling scene changes on UI buttons
    //if you wish to directly call a scene change
    //please call PyScene directly, it is located inside the GameManager object

    private ScenesSystem _scenesSystem;

    private void Start()
    {
        _scenesSystem = FindObjectOfType<ScenesSystem>();
    }

    public void ChangeScene(string sceneName)
    {
        if (sound != null) AudioSystem.Instance.PlaySilenceableNarration(sound);

        StartCoroutine(SceneCaller(sceneName));
    }

    private IEnumerator SceneCaller(string sceneName)
    {
        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0f);
        _scenesSystem.ChangeScene(sceneName);
    }

    public void ExtraAudio(AudioClip audio)
    {
        AudioSystem.Instance.PlaySilenceableSfx(audio);
    }
}