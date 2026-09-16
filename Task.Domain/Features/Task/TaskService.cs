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
        public TaskService()
        {
            _db = new HRMSDbContext();
        }

        public async Task<Result<TaskResponseModel>> GetAllTasksAsync()
        {
            Result<TaskResponseModel> model = new Result<TaskResponseModel>();

            var tasks = await _db.TaskItems.Where(x => x.DeleteFlag == false).ToListAsync();
            TaskResponseModel responseModel = new TaskResponseModel
            {
                AllTasks = tasks
            };
            model = Result<TaskResponseModel>.Success(responseModel);

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
            model = Result<TaskResponseModel>.Success(responseModel);

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

            model = Result<TaskResponseModel>.Success(responseModel, message);
            return model;
        }
    }
}
