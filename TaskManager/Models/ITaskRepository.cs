using System.Threading.Tasks;

namespace TaskManager.Models
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<Models.Task>> GetTasks();
        public Task<Models.Task> GetTaskById(int taskId);
        public Task<Task> CreateTask(Task task);
        public System.Threading.Tasks.Task UpdateTask(int id, Models.Task task);
        //public System.Threading.Tasks.Task DeleteTask(Models.Task task);
    }



}
