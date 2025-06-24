using System;
using System.Diagnostics;
using Playmove.Avatars.API.Vms;
using Playmove.Core.API.Models;

namespace Playmove.Avatars.API.Models
{
    [Serializable]
    public class SlotsConfig : VmItem<SlotsConfigVm>
    {
        public SlotsConfig()
        {
        }

        public SlotsConfig(int totalSlots, int maxPlayersPerSlot, int minSlots, bool hasAI, bool playingWithAI = false)
        {
            TotalSlots = totalSlots;
            MinSlots = minSlots;
            MaxPlayersPerSlot = maxPlayersPerSlot;
            HasAI = hasAI;
            OpenedGame = Process.GetCurrentProcess().ProcessName;
            PlayingWithAI = playingWithAI;
        }

        public int TotalSlots { get; set; }
        public int MinSlots { get; set; }
        public int MaxPlayersPerSlot { get; set; }
        public bool HasAI { get; set; }
        public string OpenedGame { get; set; }
        public bool PlayingWithAI { get; set; }

        public override SlotsConfigVm GetVm()
        {
            return new SlotsConfigVm
            {
                TotalSlots = TotalSlots,
                MinSlots = MinSlots,
                MaxPlayersPerSlot = MaxPlayersPerSlot,
                HasAI = HasAI,
                OpenedGame = OpenedGame
            };
        }

        public override void SetDataFromVm(SlotsConfigVm vm)
        {
            TotalSlots = vm.TotalSlots;
            MinSlots = vm.MinSlots;
            MaxPlayersPerSlot = vm.MaxPlayersPerSlot;
            HasAI = vm.HasAI;
            OpenedGame = vm.OpenedGame;
        }
    }
}