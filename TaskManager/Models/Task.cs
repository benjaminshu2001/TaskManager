namespace TaskManager.Models
{
    public class Task : ITask
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime DueDate { get; set; }
        public bool isCompleted { get; set; }
    }
}
