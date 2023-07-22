
namespace TaskManager.Models
{
    public interface ITaskManagerUserRepository
    {
        public Task<IEnumerable<TaskManagerUser>> GetUsers();

    }
}
