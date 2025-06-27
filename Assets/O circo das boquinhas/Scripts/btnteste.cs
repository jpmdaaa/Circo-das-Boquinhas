using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class btnteste : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanhaoController canhao;

    public void Start()
    {
        canhao = (CanhaoController)GameObject.FindObjectOfType(typeof(CanhaoController));
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Code to execute when the mouse enters the button
        canhao.podeDisparar = false;
        Debug.Log("Mouse entered button");

        // Example: Change the button's color
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canhao.podeDisparar = true;
        // Code to execute when the mouse exits the button
        Debug.Log("Mouse exited button");
        // Example: Reset the button's color
        
    }
}