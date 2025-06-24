using UnityEngine;

public class StandaloneSoundPlayer : MonoBehaviour
{
    public enum E_SoundChannel
    {
        Narration,
        Music,
        Sounds
    }

    public E_SoundChannel channel = E_SoundChannel.Narration;
    public AudioClip clip;
    public bool forceNoOverlap;
    private bool canPlay = true;

    public void PlaySound()
    { ;
        if (!canPlay)
            return;

        if (forceNoOverlap)
        {
            canPlay = false;
            Invoke("EnablePlayAgain", clip.length);
        }

        if (channel == E_SoundChannel.Narration)
            AudioSystem.Instance.PlaySilenceableNarration(clip);
        else if (channel == E_SoundChannel.Music)
            AudioSystem.Instance.PlayMusic(clip);
        else if (channel == E_SoundChannel.Sounds) AudioSystem.Instance.PlaySilenceableSfx(clip);
    }

    private void EnablePlayAgain()
    {
        canPlay = true;
    }
}