using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _TaskManagerdbContext;
        private readonly DapperContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public TaskRepository(TaskManagerDbContext taskManagerdbContext, DapperContext DapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _TaskManagerdbContext = taskManagerdbContext;
            _context = DapperContext;
            _httpContextAccessor = httpContextAccessor;
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
                var user = _httpContextAccessor.HttpContext?.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user != null && user.Identity.IsAuthenticated)
                {
                    // Get the user ID from the authenticated user's claims
                    parameters.Add("CreatedBy", userId); // Use the user ID as the CreatedBy value
                }
                else
                {
                    // Handle the case where the user is not authenticated (if needed)
                    parameters.Add("CreatedBy", "Anonymous");
                }

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
                    isCompleted = task.isCompleted,
                    CreatedBy = userId ?? "Anonymous"
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
