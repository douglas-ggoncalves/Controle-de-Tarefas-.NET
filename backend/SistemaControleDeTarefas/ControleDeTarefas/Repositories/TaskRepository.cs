using ControleDeTarefas.Data;
using ControleDeTarefas.Models;
using ControleDeTarefas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDeTarefas.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly SystemTaskDBContext _dbContext;

        public TaskRepository(SystemTaskDBContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<List<TaskModel>> BuscarTodasTarefas()
        {
            return await _dbContext.Tarefas.ToListAsync();
        }

        public async Task<TaskModel> BuscarPorID(int ID)
        {
            return await _dbContext.Tarefas.FirstOrDefaultAsync(u => u.IDTarefa == ID);
        }

        public async Task<TaskModel> Adicionar(TaskModel taskModel)
        {
            await _dbContext.Tarefas.AddAsync(taskModel);
            await _dbContext.SaveChangesAsync();

            return taskModel;
        }

        public async Task<TaskModel> Atualizar(TaskModel taskModel)
        {
            _dbContext.Tarefas.Update(taskModel);
            await _dbContext.SaveChangesAsync();

            return taskModel;
        }

        public async Task<bool> Deletar(int ID)
        {
            var usuarioDB = await BuscarPorID(ID);

            _dbContext.Tarefas.Remove(usuarioDB); 
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}