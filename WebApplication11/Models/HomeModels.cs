using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication11.Models
{
    public class ContactModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string ContactName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }

}