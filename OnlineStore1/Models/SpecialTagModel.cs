using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    public class SpecialTagModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public string SpecialTag { get; set; }

    }
}
