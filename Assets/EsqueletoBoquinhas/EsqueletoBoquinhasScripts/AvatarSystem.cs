using System.Collections.Generic;
using Playmove.Avatars.API;
using Playmove.Avatars.API.Models;
using UnityEngine;

public class AvatarSystem : MonoBehaviour
{
    public Player slot0player = new();
    public Player slot1player = new();
    public Player slot2player = new();
    public Player slot3player = new();
    private List<Slot> slots = new();

    public void CopySlotsInfoFromAPI()
    {
        slot0player = new Player();
        slot1player = new Player();
        slot2player = new Player();
        slot3player = new Player();
        slots = AvatarAPI.CurrentSlots;
        PopulateSlots();
    }

    private void PopulateSlots()
    {
        try
        {
            slot0player = slots[0].Players[0];
        }
        catch
        {
            //print("Slot 0 empty");
            //print(e.Message);
        }

        try
        {
            slot1player = slots[1].Players[0];
        }
        catch
        {
            //print("Slot 1 empty");
            //print(e.Message);
        }

        try
        {
            slot2player = slots[2].Players[0];
        }
        catch
        {
            //print("Slot 2 empty");
            //print(e.Message);
        }

        try
        {
            slot3player = slots[3].Players[0];
        }
        catch
        {
            //print("Slot 3 empty");
            //print(e.Message);
        }
    }
}