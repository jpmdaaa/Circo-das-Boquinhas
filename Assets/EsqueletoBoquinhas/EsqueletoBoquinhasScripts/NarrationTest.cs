using UnityEngine;

public class NarrationTest : MonoBehaviour
{
    public AudioClip testClip;

    public void Test()
    {
        AudioSystem.Instance.PlayUnSilenceableNarration(testClip);
    }
}