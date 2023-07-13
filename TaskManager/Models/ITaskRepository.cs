using System.Threading.Tasks;

namespace TaskManager.Models
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<Task>> GetTasks();
        public Task<Task> CreateTask(Task task);
    }



}
