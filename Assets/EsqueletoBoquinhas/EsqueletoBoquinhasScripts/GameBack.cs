using UnityEngine;
using UnityEngine.EventSystems;

public class GameBack : MonoBehaviour
{
    [Header("Gambiarra do Audio?")] public bool xgh;

    [Header("PopUp")] public GameObject backPopUp;

    public Animator popupAnimator;
    public bool activate;

    public void OpenPopUp()
    {
        activate = true;
        Time.timeScale = 0;
        backPopUp.SetActive(true);
        popupAnimator.Play("PopupOpen");
      
    }

    public void ClosePopUp()
    {
        activate = false;
        popupAnimator.Play("PopupClose");
        Time.timeScale = 1;
        Invoke("DisablePopupPanel", .5f);
      
    }

    private void DisablePopupPanel()
    {
        backPopUp.SetActive(false);
    }

    public void BackToMenu()
    {
        if (xgh) FindObjectOfType<ForceSingleAudio>().StopAll();
        Time.timeScale = 1;
    }
}

