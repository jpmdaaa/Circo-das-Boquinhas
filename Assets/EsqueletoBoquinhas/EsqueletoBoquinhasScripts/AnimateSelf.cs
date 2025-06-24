using UnityEngine;

public class AnimateSelf : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation(string animation)
    {
        anim.Play(animation);
    }
}