using Playmove.Avatars.API;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public bool isToOpenAvatar = true;
    public string nextScene = "PlayerSelect";

    private void Start()
    {
        // Call avatar select
        if (isToOpenAvatar)
            AvatarAPI.Open(result => LoadMainMenu());
        //isToOpenAvatar = false;
        else
            LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        FindObjectOfType<ScenesSystem>().ChangeScene(nextScene);
    }
}