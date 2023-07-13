namespace TaskManager.Models
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<Task>> GetTasks();
        void AddTask(Task task);
    }



}
