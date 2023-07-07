namespace TaskManager.Models
{
    public class Task : ITask
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Other properties specific to the Task table
    }
}
