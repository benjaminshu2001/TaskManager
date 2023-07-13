using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

//obtained structure from https://code-maze.com/using-dapper-with-asp-net-core-web-api/
namespace TaskManager.Models
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("TaskManagerDbContext");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
