using ControleDeTarefas.Models;

namespace ControleDeTarefas.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> BuscarTodasTarefas();

        Task<TaskModel> Adicionar(TaskModel taskModel);

        Task<TaskModel> Atualizar(TaskModel taskModel);

        Task<bool> Deletar(int ID);
    }
}