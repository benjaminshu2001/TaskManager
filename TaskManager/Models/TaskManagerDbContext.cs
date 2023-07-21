using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace TaskManager.Models
{
    public class TaskManagerDbContext : IdentityDbContext<TaskManagerUser, IdentityRole<Guid>, Guid>
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
    }
}
