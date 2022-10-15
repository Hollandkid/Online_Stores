using Microsoft.AspNetCore.Mvc;
using OnlineStore1.Data;
using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Admin.Controllers
{
    public class CloneRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CloneRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ProductModel Clone (ProductModel productModel)
        {
            return _dbContext.Products.Find(productModel.Id);
            
        }
    }
}
