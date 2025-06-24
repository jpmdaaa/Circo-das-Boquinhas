using Boquinhas.Core;
using Playmove.Avatars.API;
using UnityEngine;

public class PlayerSelectSlotCheck : MonoBehaviour
{
    public GameObject btnPlay;

    private void Update()
    {
        if (AvatarAPI.SlotsPlaying.Count > 0 && GameManager.Instance.GetCurrentDifficultyLevel() != DifficultyLevels.None)
            btnPlay.SetActive(true);
        else
            btnPlay.SetActive(false);
    }
}