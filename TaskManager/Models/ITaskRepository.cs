namespace TaskManager.Models
{
    public interface ITaskRepository
    {
        IEnumerable<Task> AllTasks { get; }

    }



}
