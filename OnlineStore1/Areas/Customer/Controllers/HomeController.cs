using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineStore1.Data;
using OnlineStore1.Models;
using OnlineStore1.Utility;
using OnlineStore1.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace OnlineStore1.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        //Get Index Action Method
        public IActionResult Index(int? page)
        {
            //I used the X.PagedList for the MVC which was download from the Nuget Manager... Also use the Method
            var result = _dbContext.Products.Include(c => c.productTypes).Include(f => f.TagTypes).ToList().ToPagedList(page ?? 1, 28);
            var catResult = _dbContext.ProductTypes.ToList();

            //This is using DOT
            var productVM = new ProductViewModel
            {
                Products = result,
                ProductTypes = catResult
            };
            if (result == null)
            {
                return NotFound();
            }

            return View(productVM);
        }



        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //Get Product Details Action Method
        public IActionResult Details(int? Id)
        {
            
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                //var result = _dbContext.Products.Find(Id);
                var product = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).FirstOrDefault(f => f.Id == Id);
                var products = _dbContext.Products.Include(c => c.productTypes).Include(f => f.TagTypes).ToList();
                var ProductsVM = new AllProductViewModel
                {
                    Product = product,
                    Products = products
                };
                if (product == null) return NotFound();
                
                return View(ProductsVM);
            }

        }

        //Post Product Details Action Method
        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> ProductDetails(int? Id)
        {
            //get the model
            List<ProductModel> productModels = new List<ProductModel>();

            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                //var result = _dbContext.Products.Find(Id);
                var product = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).FirstOrDefault(f => f.Id == Id);
                var products = await _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).ToListAsync();
                if (product == null)
                {
                    return NotFound();
                }

                productModels = HttpContext.Session.Get<List<ProductModel>>("productModel");    //This is to set the Session to the model
                if (productModels == null)
                {
                    productModels = new List<ProductModel>();

                }
                productModels.Add(product);
                HttpContext.Session.Set("productModel", productModels);     //This isto set the Session to the model

                var Allproduct = new AllProductViewModel
                {
                    Product = product,
                    Products = products
                };

                return View(Allproduct);
            }
        }



        //Get Remove ActionMethod
        public IActionResult Remove(int? Id)
        {
            //get the model
            List<ProductModel> productModels = HttpContext.Session.Get<List<ProductModel>>("productModel");

            if (productModels != null)
            {
                var product = productModels.FirstOrDefault(g => g.Id == Id);

                if (product != null)
                {
                    productModels.Remove(product);
                    HttpContext.Session.Set("productModel", productModels);
                }
            }
            return RedirectToAction("Cart");
        }

        //Post Remove ActionMethod
        [HttpPost]
        [ActionName("Remove")]
        public IActionResult RemovefromCart(int? Id)
        {
            //get the model
            List<ProductModel> productModels = HttpContext.Session.Get<List<ProductModel>>("productModel");

            if (productModels != null)
            {
                var product = productModels.FirstOrDefault(g => g.Id == Id);

                if (product != null)
                {
                    productModels.Remove(product);
                    HttpContext.Session.Set("productModel", productModels);
                }
            }
            return RedirectToAction("Cart");
        }



        //Get Product Cart Action Method
        public IActionResult Cart()
        {
            List<ProductModel> productModels = HttpContext.Session.Get<List<ProductModel>>("productModel");

            if (productModels == null)
            {
                productModels = new List<ProductModel>();
            }

            //var AllProduct = new AllProductViewModel
            //{
            //    Products = productModels
            //};

            return View(productModels);
        }

        //Get Index Action Method
        public IActionResult GetPhone(int? page,string Category,string Brand)
        {
            //I used the X.PagedList for the MVC which was download from the Nuget Manager... Also use the Method
            //return View(_dbContext.Products.Include(c => c.productTypes).Include(f => f.TagTypes).ToList().ToPagedList(page ?? 1, 2));
            
            var result = _dbContext.Products.Include(c => c.productTypes).Include(f => f.TagTypes).Where(c => c.productTypes.ProductType == Category).ToList().ToPagedList(page ?? 1, 16);
            var Catresult = _dbContext.ProductTypes.ToList();
            if (null == Category && null != Brand)
            {
                result = _dbContext.Products.Include(c => c.productTypes).Include(f => f.TagTypes).Where(c => c.TagTypes.SpecialTag == Brand).ToList().ToPagedList(page ?? 1, 16);
            }
            var VmModel = new ProductViewModel
            {
                Products = result,
                ProductTypes = Catresult
            };
            if (result == null)
            {
                return NotFound();
            }

            return View(VmModel);
        }
    }
}
