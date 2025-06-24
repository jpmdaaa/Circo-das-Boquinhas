using UnityEngine;

public class LoadAvatarSceneButton : MonoBehaviour
{
    private ScenesSystem _scenesSystem;
    public AudioClip ligaButtonSound;

    private void Start()
    {
        _scenesSystem = FindObjectOfType<ScenesSystem>();
    }

    public void LoadAvatarScene()
    {
        AudioSystem.Instance.PlaySilenceableNarration(ligaButtonSound);
        _scenesSystem.ChangeScene("AvatarReSelect");
    }
}