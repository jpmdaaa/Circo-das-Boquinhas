using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public Color selectedColor;
    public Color unselectedColor;
    public Image buttonInfo;
    public Image buttonHowToPlay;
    public Image buttonConfig;
    public GameObject contentInfo;
    public GameObject contentHowToPlay;
    public GameObject contentConfig;
    public AudioClip infoNarration;
    public AudioClip howToPlayNarration;
    public AudioClip configNarration;

    private void Start()
    {
        HideAll();
        ShowInfoContent();
    }

    public void HideAll()
    {
        buttonInfo.color = unselectedColor;
        buttonHowToPlay.color = unselectedColor;
        buttonConfig.color = unselectedColor;
        contentInfo.SetActive(false);
        contentHowToPlay.SetActive(false);
        contentConfig.SetActive(false);
    }

    public void ShowInfoContent()
    {
        HideAll();
        AudioSystem.Instance.PlaySilenceableNarration(infoNarration);
        buttonInfo.color = selectedColor;
        contentInfo.SetActive(true);
    }

    public void ShowHowToPlayContent()
    {
        HideAll();
        AudioSystem.Instance.PlaySilenceableNarration(howToPlayNarration);
        buttonHowToPlay.color = selectedColor;
        contentHowToPlay.SetActive(true);
    }

    public void ShowConfigContent()
    {
        HideAll();
        AudioSystem.Instance.PlaySilenceableNarration(configNarration);
        buttonConfig.color = selectedColor;
        contentConfig.SetActive(true);
    }
}