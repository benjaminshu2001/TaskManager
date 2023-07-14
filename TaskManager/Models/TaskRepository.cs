using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

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
                var tasks = await connection.QueryAsync<Models.Task>("TaskManager.Tasks_GetTasks", commandType: CommandType.StoredProcedure);
                return tasks.ToList();
            }
        }
        public async Task<Models.Task> GetTaskById(int taskId)
        {
            using(var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", taskId);
                var matchingTask = await connection.QuerySingleOrDefaultAsync<Models.Task>("TaskManager.Tasks_GetTaskById", parameters, commandType: CommandType.StoredProcedure);
                
                return matchingTask;
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

                var newTaskId = await connection.QuerySingleAsync<int>("TaskManager.Tasks_CreateTask", parameters, commandType: CommandType.StoredProcedure);
                
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
        public async System.Threading.Tasks.Task UpdateTask(int id, Task task)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            parameters.Add("Title", task.Title);
            parameters.Add("Description", task.Description);
            parameters.Add("DueDate", task.DueDate);
            parameters.Add("isCompleted", task.isCompleted);
            parameters.Add("Status", task.Status);

            using (var connection = _context.CreateConnection())
            {
                 await connection.ExecuteAsync("TaskManager.Tasks_UpdateTask", parameters, commandType: CommandType.StoredProcedure);
            }
                        
        }
        public async System.Threading.Tasks.Task DeleteTask(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("TaskManager.Tasks_DeleteTask", parameters, commandType: CommandType.StoredProcedure);

            }
        }
    }
}
