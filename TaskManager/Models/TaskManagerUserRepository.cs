using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TaskManager.Models
{
    public class TaskManagerUserRepository : ITaskManagerUserRepository
    {
        private readonly TaskManagerDbContext _taskManagerDbContext;
        private readonly DapperContext _dapperContext;
        private IHttpContextAccessor _httpContextAccessor;

        //dependency injection
        public TaskManagerUserRepository(TaskManagerDbContext TaskManagerDbContext, DapperContext DapperContext, HttpContextAccessor httpContextAccessor)
        {
            _taskManagerDbContext = TaskManagerDbContext;
            _dapperContext = DapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TaskManagerUser>> GetUsers()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var users = await connection.QueryAsync<TaskManagerUser>("TaskManager.Users_Get", commandType: System.Data.CommandType.StoredProcedure);
                return users.ToList();
            }
        }
            
            
            
    }
}
