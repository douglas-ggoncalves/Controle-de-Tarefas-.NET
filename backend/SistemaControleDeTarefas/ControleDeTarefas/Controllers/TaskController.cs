using ControleDeTarefas.Models;
using ControleDeTarefas.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository _taskRepository)
        {
            this._taskRepository = _taskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskModel>>> BuscarTodosTarefas()
        {
            var lista = await _taskRepository.BuscarTodasTarefas();
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<TaskModel>> Cadastrar([FromBody] TaskModel taskModel)
        {
            TaskModel task = await _taskRepository.Adicionar(taskModel);
            return Ok(task);
        }

        [HttpPut("ID")]
        public async Task<ActionResult<TaskModel>> Atualizar([FromBody] TaskModel taskModel, int ID)
        {
            TaskModel task = await _taskRepository.Atualizar(taskModel);
            return Ok(task);
        }

        [HttpDelete("ID")]
        public async Task<ActionResult<TaskModel>> Apagar(int ID)
        {
            bool result = await _taskRepository.Deletar(ID);
            return Ok(result);
        }
    }
}
