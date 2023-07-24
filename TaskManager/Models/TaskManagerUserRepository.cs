using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Claims;
using System.DirectoryServices;

namespace TaskManager.Models
{
    public class TaskManagerUserRepository : ITaskManagerUserRepository
    {
        private readonly TaskManagerDbContext _taskManagerDbContext;
        private readonly DapperContext _dapperContext;

        //dependency injection
        public TaskManagerUserRepository(TaskManagerDbContext TaskManagerDbContext, DapperContext DapperContext)
        {
            _taskManagerDbContext = TaskManagerDbContext;
            _dapperContext = DapperContext;
        }

        public async Task<IEnumerable<TaskManagerUser>> GetUsers()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var users = await connection.QueryAsync<TaskManagerUser>("TaskManager.Users_Get", commandType: System.Data.CommandType.StoredProcedure);
                return users.ToList();
            }
        }
        //7/24/2023 getting LDAP path for the authentication process
        private string LdapPath(string domain)
        {
            string path = "";
            switch (domain)
            {
                case "KSI":
                    path = "LDAP://kingstonsolutions.corp";
                    break;
                case "SZ":
                    path = "LDAP://szdc16a.sz.kingston.corp:636";
                    break;
                default:
                    path = $"LDAP://{domain}.kingston.corp";
                    break;
            }

            return path;
        }
        //7/24/2023 authentication process
        public async Task<bool> IsAuthenticated(string domainName, string userName, string password)
        {
            bool authenticated = false;
            string path = LdapPath(domainName.ToUpper().Trim());

            try
            {
                DirectoryEntry entry = new DirectoryEntry(path, domainName + "\\" + userName, password);
                Object obj = entry.NativeObject;

                DirectorySearcher search = new(entry);

                search.Filter = "(SAMAccountName=" + userName + ")";
                //search.PropertiesToLoad.Add("physicaDeliveryOfficeName");

                SearchResult result = search.FindOne();
                authenticated = result != null;

            }
            catch (Exception ex)
            {
                //throw;
            }

            return authenticated;
        }


    }
}