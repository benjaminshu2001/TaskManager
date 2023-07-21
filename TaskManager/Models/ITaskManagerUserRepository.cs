
namespace TaskManager.Models
{
    public interface ITaskManagerUserRepository
    {
        Task<IEnumerable<TaskManagerUser>> GetUsers();

    }
}
