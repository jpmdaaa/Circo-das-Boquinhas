using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIntroAudioAutoPlay : MonoBehaviour
{
    public AudioClip audioClip;
    
    private void Start()
    {
        if (audioClip != null)
        {
            AudioSystem.Instance.PlaySilenceableNarration(audioClip);
        }
    }
}
