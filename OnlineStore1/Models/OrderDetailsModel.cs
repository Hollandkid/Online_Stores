using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    //
    public class OrderDetailsModel
    {

        public int Id { get; set; }

        [Display(Name ="Order")]
        public int OrderId { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel ProductModel { get; set; }

    }
}
