namespace Playmove.Avatars.API.Vms
{
    public class SlotAlunoVm
    {
        public SlotAlunoVm(long slotId, long alunoId, AlunoVm aluno)
        {
            SlotId = slotId;
            AlunoId = alunoId;
            Aluno = aluno;
        }

        public long AlunoId { get; set; }
        public virtual AlunoVm Aluno { get; set; }
        public long SlotId { get; set; }
    }
}