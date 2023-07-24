using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TaskManager.Models
{
    public class TaskManagerUser : IdentityUser<Guid>
    {
        [Display(Name = "Unique Id")]
        public int UniqueId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Badge ID")]
        public int Badge_ID { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Domain Name")]
        public string DomainName { get; set; }
        [Display(Name = "Created By")]
        public int Created_By { get; set; }
        [Display(Name = "Created Date")]
        public DateTime? Created_Date { get; set; }
        [Display(Name = "Updated By")]
        public int Updated_By { get; set; }
        [Display(Name = "Updated Date")]
        public DateTime? Updated_Date { get; set; }
    }


}
