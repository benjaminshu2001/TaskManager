using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Data;
using Microsoft.Data.SqlClient;
namespace TaskManager.Models
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _TaskManagerdbContext;
        private readonly DapperContext _context;
        public TaskRepository(TaskManagerDbContext taskManagerdbContext, DapperContext DapperContext)
        {
            _TaskManagerdbContext = taskManagerdbContext;
            _context = DapperContext;
        }

        public async Task<IEnumerable<Models.Task>> GetTasks()
        {
            using (var connection = _context.CreateConnection())
            {
                //DynamicParameters parameters = new DynamicParameters();
                //parameters.Add("Id");


                var tasks = await connection.QueryAsync<Models.Task>("TaskManager.Tasks_GetTasks", commandType: CommandType.StoredProcedure);
                return tasks.ToList();
            }
        }
        public async Task<Task> CreateTask(Task task)
        {
            //_TaskManagerdbContext.Tasks.Add(task);
            //_TaskManagerdbContext.SaveChanges();
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Title", task.Title);
                parameters.Add("Description", task.Description);
                parameters.Add("DueDate", task.DueDate);
                parameters.Add("isCompleted", task.isCompleted);
                parameters.Add("Status", task.Status);

                var newTaskId = await connection.ExecuteAsync("TaskManager.Tasks_CreateTask", parameters, commandType: CommandType.StoredProcedure);
                var createdTask = new Task
                {
                    Id = newTaskId,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    isCompleted = task.isCompleted
                };
                return createdTask;
            }
        }
    }
}
