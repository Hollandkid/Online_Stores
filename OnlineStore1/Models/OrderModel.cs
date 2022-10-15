using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Models
{
    //This is to get the Details of the User that Wantss the Order...
    public class OrderModel
    {
        public OrderModel()
        {
            orderDetails = new List<OrderDetailsModel>();
        }

        public int Id { get; set; }

        [Display(Name = "Order Number")]
        public string OrderNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required][Display(Name ="Phone Number")]
        public string PhoneNo { get; set; }

        [Required][EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string Address { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Total Amount")]
        public string Amount { get; set; }


        public virtual List<OrderDetailsModel> orderDetails { get; set; }
    }
}
