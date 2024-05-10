using System.ComponentModel;

namespace ControleDeTarefas.Enuns
{
    public enum CodeStatusEnum
    {
        [Description("Pendente")]
        Pending = 0,

        [Description("Concluída")]
        Completed = 1,

        [Description("Em Andamento")]
        InProgress = 2
    }
}