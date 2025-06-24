using UnityEngine;

public class SceneTransitionController : MonoBehaviour
{
    public Animator anim;
    private ScenesSystem _scenesSystem;

    private void Start()
    {
        _scenesSystem = FindObjectOfType<ScenesSystem>();
        //anim = GetComponent<Animator>();
    }

    public void TransitionIn()
    {
        anim.SetTrigger("TransitionIn");
    }

    public void TransitionOut()
    {
        anim.SetTrigger("TransitionOut");
    }
}