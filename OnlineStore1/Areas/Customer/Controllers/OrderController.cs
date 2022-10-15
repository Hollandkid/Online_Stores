using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore1.Data;
using OnlineStore1.Models;
using OnlineStore1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles ="User")]
    //[Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            return View();
        }


        //Get Checkout Action Method
        public IActionResult Checkout()
        {

            return View();
        }

        //Post Checkout ActionMethod

        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderModel orderModel)
        {
            
            //if (ModelState.IsValid)
            //{
            //    var user = User.Identity.Name;
            //    if(user == null || user.Equals(""))
            //    {
            //        ModelState.AddModelError(string.Empty, "User is not Sign in, you have to login to make orders.");

            //        return View(orderModel);
            //    }

            //    var userdetails = _dbContext.ApplicationUsers.FirstOrDefault(f => f.Email == user);
            //    if(userdetails == null)
            //    {
            //        return View(orderModel);
            //    }
            //    String fullname = $"{userdetails.LastName} {userdetails.FirstName}";

            //    if((fullname.Contains(orderModel.Name)) && (userdetails.PhoneNumber.Contains(orderModel.PhoneNo)) && (userdetails.Email.Contains(orderModel.Email)))
            //    {
            //        int totalAmount = 0;
            //        List<ProductModel> productModels = HttpContext.Session.Get<List<ProductModel>>("productModel");

            //        if (productModels != null)
            //        {
            //            foreach (var productModel in productModels)
            //            {
            //                OrderDetailsModel orderDetailsModel = new OrderDetailsModel();

            //                totalAmount += Convert.ToInt32(productModel.Price);
            //                orderDetailsModel.ProductId = productModel.Id;
            //                //orderModel.orderDetails = new List<OrderDetailsModel>();
            //                orderModel.orderDetails.Add(orderDetailsModel);

            //            }
            //            orderModel.Amount = totalAmount.ToString();
            //        }

            //        orderModel.OrderNo = GetOrderNo();
            //        _dbContext.Orders.Add(orderModel);
            //        await _dbContext.SaveChangesAsync();
            //        HttpContext.Session.Set("productModel", new List<ProductModel>());
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, "BackLogging Test failed. you cannot ");
            //        return View(orderModel);
            //    }


            //}
            //else
            //{
            //    return View(orderModel);
            //}

            return View();
        }

        //To Create a Random OrderId
        public string GetOrderNo()
        {
            //string no = new Guid().ToString();
            string no = Guid.NewGuid().ToString();
            no.Replace("-", "");
            return no;
        }

        //Get the Null Page Method
        [AllowAnonymous]
        public IActionResult Nullz(string id)
        {

            return View();
        }
    }
}
