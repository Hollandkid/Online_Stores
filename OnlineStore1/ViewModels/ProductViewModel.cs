using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace OnlineStore1.ViewModels
{
    public class ProductViewModel
    {
        public IPagedList<ProductModel> Products { get; set; }
        public List<ProductTypes> ProductTypes { get; set; }
    }
}
