namespace TaskManager.Models
{
    public interface ITask
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Category{ get; set; }
        DateTime DueDate { get; set; }
        bool isCompleted { get; set; }

    }



}
