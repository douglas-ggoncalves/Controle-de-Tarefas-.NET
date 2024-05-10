using ControleDeTarefas.Enuns;

namespace ControleDeTarefas.Models
{
    public class TaskModel
    {
        public int IDTarefa { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public CodeStatusEnum CodigoStatus { get; set; }

        public int CodigoCategoria { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }
    }
}