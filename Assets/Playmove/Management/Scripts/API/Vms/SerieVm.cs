using System.Collections.Generic;

namespace Playmove.Management.API.Vms
{
    public class SerieVm
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public virtual List<SerieLocalizacaoVm> Localizacoes { get; set; }
        public int Ordem { get; set; }
    }
}