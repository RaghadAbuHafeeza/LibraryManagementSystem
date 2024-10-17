using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LMS_LibraryManagementSystem_.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30)]
        public string Address { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string ProfilePicturePath { get; set; } 

    }
}
