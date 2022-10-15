using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.ViewModels
{
    public class AllProductViewModel
    {
        public ProductModel Product { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
