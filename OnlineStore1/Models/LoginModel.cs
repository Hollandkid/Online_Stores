using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Address is Required")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }

    }
}
