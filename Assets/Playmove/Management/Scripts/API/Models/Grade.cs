using System;
using System.Collections.Generic;
using Playmove.Core;
using Playmove.Core.API.Models;
using Playmove.Management.API.Vms;

namespace Playmove.Management.API.Models
{
    [Serializable]
    public class Grade : VmItem<SerieVm>, IDatabaseItem
    {
        public string Name { get; set; }
        public List<SerieLocalizacaoVm> Localizations { get; set; } = new();
        public int Order { get; set; }

        public string LocalizedName
        {
            get
            {
                var localized = Localizations.Find(loc => loc.Localizacao.ToLower() == GameSettings.Language.ToLower());
                if (localized == null) return Name;
                return localized.Descricao;
            }
        }

        public override SerieVm GetVm()
        {
            return new SerieVm
            {
                Id = Id,
                Nome = Name,
                Localizacoes = Localizations,
                Ordem = Order
            };
        }

        public override void SetDataFromVm(SerieVm vm)
        {
            Id = vm.Id;
            Name = vm.Nome;
            Localizations = vm.Localizacoes;
            Order = vm.Ordem;
        }
    }
}