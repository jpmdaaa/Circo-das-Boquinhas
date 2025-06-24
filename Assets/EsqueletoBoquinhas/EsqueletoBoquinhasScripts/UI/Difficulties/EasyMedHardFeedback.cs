using System;
using Boquinhas.Core;
using Playmove.Core.Controls;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EasyMedHardFeedback : MonoBehaviour
{
    public GameObject buttonEasyContainer;
    public GameObject buttonMediumContainer;
    public GameObject buttonHardContainer;
    public GameObject buttonMouthsContainer;
    public GameObject buttonLettersContainer;
    
    //This whole crap needs a overhaul but we never have time, good luck if you need to add more stuff here!
    public GameObject buttonFiguresContainer;
    public GameObject buttonMouthsImagesAltContainer;
    public GameObject buttonLettersImagesAltContainer;

    public Image buttonEasyImage;
    public Image buttonMediumImage;
    public Image buttonHardImage;
    public Image buttonMouthsImage;
    public Image buttonLettersImage;
    
    //
    public Image buttonFiguresImage;
    public Image buttonMouthsImagesAltImage;
    public Image buttonLettersImagesAltImage;

    public float buttonSelectedScale = 1.2f;
    public float buttonNotSelectedScale = 0.85f;

    public float buttonNotSelectedOpacity = 0.75f;

    
    //method that runs when the scene is loaded
    private void Start()
    {
        GameManager.Instance.SetDifficultyLevel(FindObjectOfType<DifficultySaveManager>().GetSavedDifficultyLevel());
        SelectDifficultyOption();
    }
    
    public void SelectDifficultyOption()
    {
        DifficultyLevels runningDifficultyLevel = GameManager.Instance.GetCurrentDifficultyLevel();
//        Debug.Log("Running difficulty level: " + runningDifficultyLevel);
        switch (runningDifficultyLevel)
        {
            case DifficultyLevels.None:
                buttonEasyContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonMediumContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonHardContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonEasyImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, 1);
                buttonMediumImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, 1);
                buttonHardImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, 1);
                break;
            case DifficultyLevels.Easy:
                buttonEasyContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                buttonMediumContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonHardContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonEasyImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, 1);
                buttonMediumImage.color = new Color(buttonMediumImage.color.r, buttonMediumImage.color.g,
                    buttonMediumImage.color.b, buttonNotSelectedOpacity);
                buttonHardImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, buttonNotSelectedOpacity);
                GameManager.Instance.SetDifficultyLevel(DifficultyLevels.Easy);
                FindObjectOfType<DifficultySaveManager>().SetSavedDifficultyLevel(GameManager.Instance.GetCurrentDifficultyLevel());
                break;
            case DifficultyLevels.Medium:
                buttonEasyContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonMediumContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                buttonHardContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonEasyImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonMediumImage.color = new Color(buttonMediumImage.color.r, buttonMediumImage.color.g,
                    buttonMediumImage.color.b, 1);
                buttonHardImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, buttonNotSelectedOpacity);
                GameManager.Instance.SetDifficultyLevel(DifficultyLevels.Medium);
                FindObjectOfType<DifficultySaveManager>().SetSavedDifficultyLevel(GameManager.Instance.GetCurrentDifficultyLevel());
                break;
            case DifficultyLevels.Hard:
                buttonEasyContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonMediumContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonHardContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                buttonEasyImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonMediumImage.color = new Color(buttonMediumImage.color.r, buttonMediumImage.color.g,
                    buttonMediumImage.color.b, buttonNotSelectedOpacity);
                buttonHardImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, 1);
                GameManager.Instance.SetDifficultyLevel(DifficultyLevels.Hard);
                FindObjectOfType<DifficultySaveManager>().SetSavedDifficultyLevel(GameManager.Instance.GetCurrentDifficultyLevel());
                break;
            case DifficultyLevels.Mouths:
                buttonLettersContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonFiguresContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonLettersImagesAltContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                
                buttonMouthsContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                buttonMouthsImagesAltContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                
                buttonLettersImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonLettersImagesAltImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonFiguresImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonMouthsImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, 1);
                buttonMouthsImagesAltImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, 1);
                
                GameManager.Instance.SetDifficultyLevel(DifficultyLevels.Mouths);
                FindObjectOfType<DifficultySaveManager>().SetSavedDifficultyLevel(GameManager.Instance.GetCurrentDifficultyLevel());
                break;
            case DifficultyLevels.Letters:
                buttonMouthsContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonFiguresContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonMouthsImagesAltContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                
                buttonLettersContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                buttonLettersImagesAltContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                
                buttonMouthsImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonMouthsImagesAltImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonFiguresImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonLettersImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, 1);
                buttonLettersImagesAltImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, 1);
                
                GameManager.Instance.SetDifficultyLevel(DifficultyLevels.Letters);
                FindObjectOfType<DifficultySaveManager>().SetSavedDifficultyLevel(GameManager.Instance.GetCurrentDifficultyLevel());
                break;
            case DifficultyLevels.Figures:
                buttonMouthsImagesAltContainer.transform.localScale = new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonLettersImagesAltContainer.transform.localScale =  new Vector3(buttonNotSelectedScale, buttonNotSelectedScale,
                    buttonNotSelectedScale);
                buttonFiguresContainer.transform.localScale =
                    new Vector3(buttonSelectedScale, buttonSelectedScale, buttonSelectedScale);
                
                buttonMouthsImagesAltImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonLettersImagesAltImage.color = new Color(buttonEasyImage.color.r, buttonEasyImage.color.g,
                    buttonEasyImage.color.b, buttonNotSelectedOpacity);
                buttonFiguresImage.color = new Color(buttonHardImage.color.r, buttonHardImage.color.g,
                    buttonHardImage.color.b, 1);
                
                GameManager.Instance.SetDifficultyLevel(DifficultyLevels.Figures);
                FindObjectOfType<DifficultySaveManager>().SetSavedDifficultyLevel(GameManager.Instance.GetCurrentDifficultyLevel());
                break;
        }
//        Debug.Log(GameManager.Instance.GetCurrentDifficultyLevel());
    }
}