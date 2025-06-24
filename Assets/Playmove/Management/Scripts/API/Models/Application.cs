using System;
using Playmove.Core.API.Models;
using Playmove.Management.API.Vms;

namespace Playmove.Management.API.Models
{
    [Serializable]
    public class Application : VmItem<AplicativoVm>, IDatabaseItem
    {
        public Application()
        {
        }

        public Application(AplicativoVm vm)
        {
            SetDataFromVm(vm);
        }

        public Application(string vmJson)
        {
            SetDataFromVmJson(vmJson);
        }

        public string GUID { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public string FolderPath { get; set; }
        public string Path { get; set; }

        public override AplicativoVm GetVm()
        {
            return new AplicativoVm
            {
                Id = Id,
                ProdutoGuid = GUID,
                NomeApp = Name,
                Executavel = Path,
                Path = FolderPath,
                Visivel = IsVisible
            };
        }

        public override void SetDataFromVm(AplicativoVm vm)
        {
            Id = vm.Id;
            GUID = vm.ProdutoGuid;
            Name = vm.NomeApp;
            Path = vm.Executavel;
            FolderPath = vm.Path;
            IsVisible = vm.Visivel;
        }
    }
}