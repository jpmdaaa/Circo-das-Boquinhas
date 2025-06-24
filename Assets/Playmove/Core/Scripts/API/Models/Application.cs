using System;
using Playmove.Core.API.Vms;

namespace Playmove.Core.API.Models
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
        public string Path { get; set; }

        public override AplicativoVm GetVm()
        {
            return new AplicativoVm
            {
                Id = Id,
                ProdutoGuid = GUID,
                NomeApp = Name
            };
        }

        public override void SetDataFromVm(AplicativoVm vm)
        {
            Id = vm.Id;
            GUID = vm.ProdutoGuid;
            Name = vm.NomeApp;
        }
    }
}