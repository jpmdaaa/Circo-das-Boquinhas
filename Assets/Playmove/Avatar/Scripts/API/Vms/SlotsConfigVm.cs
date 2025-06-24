namespace Playmove.Avatars.API.Vms
{
    public class SlotsConfigVm
    {
        public int TotalSlots { get; set; }
        public int MinSlots { get; set; }
        public int MaxPlayersPerSlot { get; set; }
        public bool HasAI { get; set; }
        public string OpenedGame { get; set; }
    }
}