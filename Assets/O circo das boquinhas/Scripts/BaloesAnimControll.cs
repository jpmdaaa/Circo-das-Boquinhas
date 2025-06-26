using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloesAnimControll : MonoBehaviour
{
    public string type;
    public Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        if (!gameObject.activeInHierarchy) return;

        anim.Rebind(); // força reiniciar estado
        anim.Update(0f); // aplica imediatamente
        anim.SetTrigger(type);
    }
}
