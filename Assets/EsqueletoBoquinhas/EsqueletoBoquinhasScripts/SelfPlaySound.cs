using UnityEngine;

public class SelfPlaySound : MonoBehaviour
{
    public void PlaySFX(AudioClip audio)
    {
        AudioSystem.Instance.PlaySilenceableSfx(audio);
    }

    public void PlayNarration(AudioClip audio)
    {
        AudioSystem.Instance.PlaySilenceableNarration(audio);
    }
}