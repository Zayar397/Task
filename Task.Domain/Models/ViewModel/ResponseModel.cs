using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Data.Models;

namespace Task.Domain.Models.ViewModel
{
    public class TaskResponseModel
    {
        public List<TaskItem>? AllTasks { get; set; }
        public TaskItem? Task { get; set; }
    }
}
