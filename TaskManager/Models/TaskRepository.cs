using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Net.Mime;

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
                var tasks = await connection.QueryAsync<Models.Task>("TaskManager.Tasks_Get", commandType: CommandType.StoredProcedure);
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

                //commented stuff works for Asp.Net.Identity users, don't rec in the future
                //var user = _httpContextAccessor.HttpContext?.User;
                //var userName = user.FindFirst(ClaimTypes.Name)?.Value;
                //7/25/2023 changed HttpContextAccessor to PrincipalContext

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, "bshu");
                string samAccountName = "";

                if (user != null)
                {
                    samAccountName = user.SamAccountName;
                    // Get the user ID from the authenticated user's claims
                    parameters.Add("CreatedBy", samAccountName); // Use the user ID as the CreatedBy value
                    parameters.Add("UpdatedBy", samAccountName);
                }
                else
                {
                    // Handle the case where the user is not authenticated (if needed)
                    parameters.Add("CreatedBy", "Anonymous");
                    parameters.Add("UpdatedBy", "Anonymous");

                }

                parameters.Add("Title", task.Title);
                parameters.Add("Description", task.Description);
                parameters.Add("DueDate", task.DueDate);
                parameters.Add("isCompleted", task.isCompleted);
                parameters.Add("Status", task.Status);

                var newTaskId = await connection.QuerySingleAsync<int>("TaskManager.Tasks_Create", parameters, commandType: CommandType.StoredProcedure);

                var createdTask = new Task
                {
                    Id = newTaskId,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    isCompleted = task.isCompleted,
                    CreatedBy = samAccountName ?? "Anonymous",
                    UpdatedBy = samAccountName ?? "Anonymous"
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

            //7/25/2023 changed HttpContextAccessor to PrincipalContext
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, "bshu");
            string samAccountName = "";

            if (user != null)
            {
                samAccountName = user.SamAccountName;
                // Get the user ID from the authenticated user's claims
                parameters.Add("UpdatedBy", samAccountName);
            }
            else
            {
                // Handle the case where the user is not authenticated (if needed)
                parameters.Add("UpdatedBy", "Anonymous");

            }

            using (var connection = _context.CreateConnection())
            {
                 await connection.ExecuteAsync("TaskManager.Tasks_Update", parameters, commandType: CommandType.StoredProcedure);
            }
                        
        }
        public async System.Threading.Tasks.Task DeleteTask(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("TaskManager.Tasks_Delete", parameters, commandType: CommandType.StoredProcedure);

            }
        }
    }
}
