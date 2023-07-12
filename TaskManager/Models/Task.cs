using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Task 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Due Date"), DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [Display(Name = "Is Completed")]
        public bool isCompleted { get; set; }
        public string Status { get; set; }

        public void UpdateStatus()
        {
            Status = isCompleted ? "Completed" : "In progress";
        }
    }
}
