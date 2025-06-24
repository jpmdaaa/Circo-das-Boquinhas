using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    public AudioClip parabens;

    private void Start()
    {
        AudioSystem.Instance.PlaySilenceableNarration(parabens);
    }

    private void Update()
    {
    }
}