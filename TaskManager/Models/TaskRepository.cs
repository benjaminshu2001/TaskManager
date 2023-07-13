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
                //parameters.Add("Title");
                //parameters.Add("Description");
                //parameters.Add("DueDate");
                //parameters.Add("isCompleted");
                //parameters.Add("Status");

                var tasks = await connection.QueryAsync<Models.Task>("TaskManager.Tasks_GetTasks", commandType: CommandType.StoredProcedure);
                return tasks.ToList();
            }
        }
        public void AddTask(Task task)
        {
            _TaskManagerdbContext.Tasks.Add(task);
            _TaskManagerdbContext.SaveChanges();
        }
    }
}
