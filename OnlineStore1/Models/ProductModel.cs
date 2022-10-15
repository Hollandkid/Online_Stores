using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    public class ProductModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        //public int Price { get; set; }

        public String Image { get; set; }

        [Required, Display(Name ="Product Color")]
        public string ProductColor { get; set; }

        [Required, Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Required, Display(Name = "Category")]
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductTypes productTypes { get; set; }

        [Required, Display(Name = "Brand")]
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public SpecialTagModel TagTypes { get; set; }

        [Required, Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
