using Microsoft.AspNetCore.Identity;
using System;
using System.Drawing;

namespace TaskManager.Models
{
    public class TaskManagerUser : IdentityUser<Guid>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Badge_ID { get; set; }
        public bool IsActive { get; set; }
        public string DomainName { get; set; }
        public int Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int Updated_By { get; set; }
        public DateTime? Updated_Date { get; set; }

    }


}
