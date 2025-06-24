using Boquinhas.Core;
using UnityEngine;

public class GameOptionsManager : MonoBehaviour
{
    public GameObject playButton;

    private void Awake()
    {
        //Seleciono a dificuldade Easy como a padrão para quando a tela abrir
        SetDifficulty(4);
    }

    public void SetDifficulty(int difficulty)
    {
        GameManager.Instance.SetDifficultyLevel((DifficultyLevels)difficulty);
        playButton.SetActive(true);
    }
}