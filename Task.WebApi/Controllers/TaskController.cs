using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Task.Data.Data;
using Task.Data.Models;

namespace Task.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly HRMSDbContext _db;
        public TaskController()
        {
            _db = new HRMSDbContext();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _db.TaskItems.Where(x => x.DeleteFlag == false).ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskItem task)
        {
            await _db.TaskItems.AddAsync(task);
            int recCount = await _db.SaveChangesAsync();
            return Ok(new {Message = recCount > 0 ? "Record inserted successfully." : "Record inserted failed." });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            var taskItem = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);
            if (taskItem == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(task.Title))
            {
                taskItem.Title = task.Title;
            }
            if (!string.IsNullOrEmpty(task.Description))
            {
                taskItem.Description = task.Description;
            }
            if (!string.IsNullOrEmpty(task.Status))
            {
                taskItem.Status = task.Status;
            }
            if (!string.IsNullOrEmpty(task.Priority))
            {
                taskItem.Priority = task.Priority;
            }
            if (task.DueDate.HasValue && task.DueDate != DateTime.MinValue)
            {
                taskItem.DueDate = task.DueDate;
            }

            _db.Entry(taskItem).State = EntityState.Modified;
            int recCount = await _db.SaveChangesAsync();

            return Ok(new { Message = recCount > 0 ? "Record updated successfully." : "Record updated failed." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var taskItem = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);
            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.DeleteFlag = true;

            _db.Entry(taskItem).State = EntityState.Modified;
            int recCount = await _db.SaveChangesAsync();

            return Ok(new { Message = recCount > 0 ? "Record deleted successfully." : "Record deleted failed." });

        }
    }
}
