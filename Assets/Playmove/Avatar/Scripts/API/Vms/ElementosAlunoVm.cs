namespace Playmove.Avatars.API.Vms
{
    public class ElementosAlunoVm
    {
        public ElementosAlunoVm(long alunoId, long elementoId, ElementoVm elemento)
        {
            AlunoId = alunoId;
            ElementoId = elementoId;
            Elemento = elemento;
        }

        public long AlunoId { get; set; }
        public virtual AlunoVm Aluno { get; set; }
        public long ElementoId { get; set; }
        public virtual ElementoVm Elemento { get; set; }
    }
}