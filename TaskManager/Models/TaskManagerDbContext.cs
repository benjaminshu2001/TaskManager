using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Models
{
    public class TaskManagerDbContext : IdentityDbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }

    }
}
