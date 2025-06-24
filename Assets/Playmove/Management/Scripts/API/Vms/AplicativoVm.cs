namespace Playmove.Management.API.Vms
{
    public class AplicativoVm
    {
        public long Id { get; set; }
        public string ProdutoGuid { get; set; }
        public string NomeApp { get; set; }
        public bool Visivel { get; set; }
        public string Path { get; set; }
        public string Executavel { get; set; }
    }
}