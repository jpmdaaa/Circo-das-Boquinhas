using System;
using Playmove.Core.API.Models;
using Playmove.Core.API.Vms;

namespace Playmove.Management.API.Models
{
    [Serializable]
    public class Info : VmItem<PlaytableInfoVm>, IDatabaseItem
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public override PlaytableInfoVm GetVm()
        {
            return new PlaytableInfoVm
            {
                Id = Id,
                Nome = Name,
                Email = Mail,
                Telefone = PhoneNumber,
                Cidade = City,
                Estado = State
            };
        }

        public override void SetDataFromVm(PlaytableInfoVm vm)
        {
            Id = vm.Id;
            Name = vm.Nome;
            Mail = vm.Email;
            PhoneNumber = vm.Telefone;
            City = vm.Cidade;
            State = vm.Estado;
        }

        public override string ToString()
        {
            return $"(Id: {Id}; Name: {Name})";
        }
    }
}