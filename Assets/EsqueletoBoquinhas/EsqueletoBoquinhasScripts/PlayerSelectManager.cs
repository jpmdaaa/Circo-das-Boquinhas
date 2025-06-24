using UnityEngine;

public class PlayerSelectManager : MonoBehaviour
{
    public PlayerIcon slot0, slot1, slot2, slot3;
    //public AudioClip clip;
    private AvatarSystem _avatarSystem;

    private void Start()
    {
        _avatarSystem = FindObjectOfType<AvatarSystem>();
        _avatarSystem.CopySlotsInfoFromAPI();
        PopulateSlots();
        //AudioSystem.Instance.PlaySilenceableNarration(clip);
    }

    private void PopulateSlots()
    {
        slot0.SetupSlot(_avatarSystem.slot0player.Name, _avatarSystem.slot0player.ThumbnailPath);
        slot1.SetupSlot(_avatarSystem.slot1player.Name, _avatarSystem.slot1player.ThumbnailPath);
        slot2.SetupSlot(_avatarSystem.slot2player.Name, _avatarSystem.slot2player.ThumbnailPath);
        slot3.SetupSlot(_avatarSystem.slot3player.Name, _avatarSystem.slot3player.ThumbnailPath);
    }
}