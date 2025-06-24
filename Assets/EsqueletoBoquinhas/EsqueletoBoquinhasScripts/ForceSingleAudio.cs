using System.Collections.Generic;
using UnityEngine;

public class ForceSingleAudio : MonoBehaviour
{
    public List<AudioSource> audioSources;

    public bool AnotherAudioIsPlaying()
    {
        var isAnotherPlaying = false;
        foreach (var audioSource in audioSources)
            if (audioSource.isPlaying)
                isAnotherPlaying = true;

        if (AudioSystem.Instance.unSilenceableNarrationChannel.isPlaying) isAnotherPlaying = true;

        return isAnotherPlaying;
    }

    public void StopAll()
    {
        foreach (var audioSource in audioSources) audioSource.Stop();
    }
}