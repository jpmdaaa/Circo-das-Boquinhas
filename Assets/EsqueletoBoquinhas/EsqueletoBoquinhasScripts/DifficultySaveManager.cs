using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySaveManager : MonoBehaviour
{
    private DifficultyLevels _savedDifficultyLevel = DifficultyLevels.None;
    
    public DifficultyLevels GetSavedDifficultyLevel()
    {
        //Debug.Log("Getting saved difficulty level: " + _savedDifficultyLevel);
        return _savedDifficultyLevel;
    }
    
    public void SetSavedDifficultyLevel(DifficultyLevels difficultyLevel)
    {
        //Debug.Log("Setting saved difficulty level to " + difficultyLevel);
        _savedDifficultyLevel = difficultyLevel;
    }
    
    public void ResetSavedDifficultyLevel()
    {
        //Debug.Log("Resetting saved difficulty level.");
        _savedDifficultyLevel = DifficultyLevels.None;
    }
}
