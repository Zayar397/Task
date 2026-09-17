using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Task.Data.Data;
using Task.Data.Models;
using Task.Domain.Features.Task;

namespace Task.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var model = await _taskService.GetAllTasksAsync();
            return ReturnStatus(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var model = await _taskService.GetTaskByIdAsync(id);
            return ReturnStatus(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskItem task)
        {
            var model = await _taskService.AddTaskAsync(task);
            return ReturnStatus(model);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            var model = await _taskService.UpdateTaskAsync(id, task);
            return ReturnStatus(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var model = await _taskService.DeleteTaskAsync(id);
            return ReturnStatus(model);
        }
    }
}
