
namespace TaskManager.Models
{
    public interface ITaskManagerUserRepository
    {
        public Task<IEnumerable<TaskManagerUser>> GetUsers();
        //7/24/2023 working on authenticating user based on AD information
        public Task<bool> IsAuthenticated(string domainName, string userName, string password);
    }
}
