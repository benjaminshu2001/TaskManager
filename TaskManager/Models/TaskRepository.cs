using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _TaskManagerdbContext;

        public TaskRepository(TaskManagerDbContext taskManagerdbContext)
        {
            _TaskManagerdbContext = taskManagerdbContext;
        }
        public IEnumerable<Task> AllTasks => _TaskManagerdbContext.Tasks;

        public void AddTask(Task task)
        {
            _TaskManagerdbContext.Tasks.Add(task);
            _TaskManagerdbContext.SaveChanges();
        }
    }
}
