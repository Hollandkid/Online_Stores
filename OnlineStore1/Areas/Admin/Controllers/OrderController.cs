using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var result = _dbContext.Orders.ToList();
            return View(result);
        }


        //Get the Null Page Method
        [AllowAnonymous]
        public IActionResult Nullz(string id)
        {
            
            return View();
        }
        
    }
}
