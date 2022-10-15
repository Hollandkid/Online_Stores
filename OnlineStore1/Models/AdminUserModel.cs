using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    public class AdminUserModel
    {

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter your Password")]
        [Display(Name = "Password")]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please confirm you Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        [Required]
        [Display(Name = "Home/Contact Address")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
