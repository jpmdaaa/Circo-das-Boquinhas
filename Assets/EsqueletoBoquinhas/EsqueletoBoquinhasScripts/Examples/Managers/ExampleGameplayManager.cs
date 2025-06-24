using TMPro;
using UnityEngine;

public class ExampleGameplayManager : MonoBehaviour
{
    public TMP_Text text;
    private int score;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        print("Setup");
        score = 0;
    }

    public void Correct()
    {
        print("Correct");
        score = score + 1;
        UpdateUI();
    }

    public void Incorrect()
    {
        print("Incorrect");
        score = score - 1;
        UpdateUI();
    }

    public void EndGame()
    {
        print("Ending game");
        FindObjectOfType<ScenesSystem>().ChangeScene("GameEnd");
    }

    private void UpdateUI()
    {
        text.text = "SCORE: " + score;
    }
}