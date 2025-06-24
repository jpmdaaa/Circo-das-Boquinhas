using Playmove.Avatars.API;
using UnityEngine;

public class AvatarReselectManager : MonoBehaviour
{
    public static bool isToOpenAvatar = true;

    private void Start()
    {
        // Call avatar select
        if (isToOpenAvatar)
            AvatarAPI.Open(result => LoadPlayerSelect());
        //isToOpenAvatar = false;
    }

    private void LoadPlayerSelect()
    {
        FindObjectOfType<ScenesSystem>().ChangeScene("PlayerSelect");
    }
}