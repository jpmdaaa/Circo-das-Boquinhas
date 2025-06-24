using System;
using Playmove.Core.API.Vms;

namespace Playmove.Core.API.Models
{
    [Serializable]
    public class RawFile : VmItem<ArquivoVm>, IDatabaseItem
    {
        private string _fullPath = string.Empty;

        public RawFile()
        {
        }

        public RawFile(ArquivoVm vm)
        {
            SetDataFromVm(vm);
        }

        public RawFile(string vmJson)
        {
            SetDataFromVmJson(vmJson);
        }

        public string Name { get; set; }
        public string Language { get; set; }
        public float Size { get; set; }
        public string Extension { get; set; }

        public string FullPath
        {
            get => _fullPath;
            set => _fullPath = value.Replace(@"\", "/");
        }

        public override ArquivoVm GetVm()
        {
            return new ArquivoVm
            {
                Id = Id,
                Nome = Name,
                Localizacao = Language,
                Extensao = Extension,
                Tamanho = Size,
                CaminhoArquivo = FullPath
            };
        }

        public override void SetDataFromVm(ArquivoVm vm)
        {
            Id = vm.Id;
            Name = vm.Nome;
            Language = vm.Localizacao;
            Extension = vm.Extensao;
            Size = vm.Tamanho;
            FullPath = vm.CaminhoArquivo;
        }
    }
}