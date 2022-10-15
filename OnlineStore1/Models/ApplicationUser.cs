using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    //This is a Model for the User Login and other Functions containing to the User Account... It was also added to the DbContext class...
    //Seen the ApplicationUser was Inherited from the IdentityUser Class... then you have to Replace ApplicationUser for IdentityUser in all places where the IdentityUser was called
    public class ApplicationUser : IdentityUser
    {
        [Required][Display (Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm you Password")]
        [Compare("Password",ErrorMessage ="Password Does not Macth")]
        [Display(Name = "Confirm Password")]
        public virtual string ConfirmPassword { get; set; }


        [Required]
        [Display(Name = "Contact/Delivery Address")]
        public string Address { get; set; }
    }
}
