namespace TaskManager.Models
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _TaskManagerdbContext;

        public TaskRepository(TaskManagerDbContext taskManagerdbContext)
        {
            _TaskManagerdbContext = taskManagerdbContext;
        }
        public IEnumerable<Task> AllTasks { get; } = default!;
        //public IEnumerable<Task> AllTasks => _TaskManagerdbContext.Tasks.OrderBy(t => t.Title);
    }
}
