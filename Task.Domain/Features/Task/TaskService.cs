using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task.Data.Data;
using Task.Data.Models;
using Task.Domain.Models;
using Task.Domain.Models.ViewModel;

namespace Task.Domain.Features.Task
{
    public class TaskService
    {
        private readonly HRMSDbContext _db;

        public TaskService(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<Result<TaskResponseModel>> GetAllTasksAsync()
        {
            Result<TaskResponseModel> model = new Result<TaskResponseModel>();

            var tasks = await _db.TaskItems.Where(x => x.DeleteFlag == false).ToListAsync();
            TaskResponseModel responseModel = new TaskResponseModel
            {
                AllTasks = tasks
            };
            model = Result<TaskResponseModel>.Success("Record exist.",responseModel);

            return model;
        }

        public async Task<Result<TaskResponseModel>> GetTaskByIdAsync(int id)
        {
            Result<TaskResponseModel> model = new Result<TaskResponseModel>();

            var task = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);
            if (task is null)
            {
                model = Result<TaskResponseModel>.ValidationError("Record does not exist.");
                goto Result;
            }

            TaskResponseModel responseModel = new TaskResponseModel
            {
                Task = task
            };
            model = Result<TaskResponseModel>.Success("Record exist.",responseModel);

        Result:
            return model;
        }

        public async Task<Result<TaskResponseModel>> AddTaskAsync(TaskItem item)
        {
            Result<TaskResponseModel> model = new Result<TaskResponseModel>();

            await _db.TaskItems.AddAsync(item);
            int recCount = await _db.SaveChangesAsync();

            string message = recCount > 0 ? "Record inserted successfully." : "Record inserted failed.";

            TaskResponseModel responseModel = new TaskResponseModel
            {
                Task = item
            };

            model = Result<TaskResponseModel>.Success(message, responseModel);
            return model;
        }

        public async Task<Result<TaskResponseModel>> UpdateTaskAsync(int id,TaskItem task)
        {
            Result<TaskResponseModel> model = new Result<TaskResponseModel>();

            var taskItem = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);
            if (taskItem == null)
            {
                model = Result<TaskResponseModel>.ValidationError("Record does not exist.");
                goto Result;
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

            string message = recCount > 0 ? "Record updated successfully." : "Record updated failed.";

            TaskResponseModel taskResponseModel = new TaskResponseModel
            {
                Task = taskItem
            };

            model = Result<TaskResponseModel>.Success(message, taskResponseModel);

        Result:
            return model;
        }

        public async Task<Result<TaskResponseModel>> DeleteTaskAsync(int id)
        {
            Result<TaskResponseModel> model = new Result<TaskResponseModel>();

            var taskItem = await _db.TaskItems.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);
            if (taskItem == null)
            {
                model = Result<TaskResponseModel>.ValidationError("Record does not exist.");
                goto Result;
            }

            taskItem.DeleteFlag = true;

            _db.Entry(taskItem).State = EntityState.Modified;
            int recCount = await _db.SaveChangesAsync();

            string message = recCount > 0 ? "Record deleted successfully." : "Record deleted failed.";

            model = Result<TaskResponseModel>.Success(message);

        Result:
            return model;
        }
    }
}
