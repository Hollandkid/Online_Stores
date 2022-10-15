using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore1.Data;
using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _dbContext;
        private ApplicationDbContext _dbContext;
        private readonly CloneRepository _cloneRepository;
        private readonly IHostingEnvironment _hostingEnvironment;   //This is to get the Environment Service

        public ProductController(ApplicationDbContext dbContext, CloneRepository cloneRepository, IHostingEnvironment hostingEnvironment)
        {
           _dbContext = dbContext;
            _cloneRepository = cloneRepository;
            _hostingEnvironment = hostingEnvironment;
        }


        //Get for Index Method
        public IActionResult Index()
        {
            //This is to return the List of the Product from the Database...
            var result = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).ToList();
            return View(result);
        }

        //Post Index Method
        [HttpPost]
        public async Task<IActionResult> Index(decimal? lowAmount, decimal? highAmount)
        {
           
                //This is to Search for a Product with a Range... Like a Filter
                var searchRange = _dbContext.Products.Include(g => g.productTypes).Include(f => f.TagTypes)
                    .Where(c => c.Price >= lowAmount && c.Price <= highAmount).ToList();
                if (lowAmount == null || highAmount ==null )
                {
                    searchRange = _dbContext.Products.Include(g => g.productTypes).Include(f => f.TagTypes).ToList();
                }
            return View(searchRange);

        }

        //Get for Create Method
        public IActionResult Create()
        {
            ViewData["productTypeid"] = new SelectList(_dbContext.ProductTypes.ToList(), "Id", "ProductType");   //This is to populate the ListView from the Database
            ViewData["specialTagid"] = new SelectList(_dbContext.SpecialTags.ToList(), "Id", "SpecialTag");
            return View();
        }

        //Post for Create Method
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel productModel,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                //This is to Check if a product alredy exist in the database
                var searchResult = _dbContext.Products.FirstOrDefault(c => c.Name == productModel.Name);
                if (searchResult != null )
                {
                    ViewBag.Duplicate = "Product Name already Exist";
                    ViewData["productTypeid"] = new SelectList(_dbContext.ProductTypes.ToList(), "Id", "ProductType");   
                    ViewData["specialTagid"] = new SelectList(_dbContext.SpecialTags.ToList(), "Id", "SpecialTag");
                    return View(productModel);
                }
                if (image != null)
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    productModel.Image = "Images/" + image.FileName;
                }
                else
                {
                    productModel.Image = "Images/profile.png";
                }
                await _dbContext.Products.AddAsync(productModel);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(productModel);
        }



        //Get for Edit Method
        public IActionResult Edit(int? Id)
        {
            ViewData["productTypeid"] = new SelectList(_dbContext.ProductTypes.ToList(), "Id", "ProductType");   //This is to populate the ListView from the Database
            ViewData["specialTagid"] = new SelectList(_dbContext.SpecialTags.ToList(), "Id", "SpecialTag");

            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                //var reusult = _dbContext.Products.Find(Id);
                var result = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).FirstOrDefault(f => f.Id == Id);

                if (result==null)
                {
                    return NotFound();
                }
                else
                {
                    return View(result);

                }
            }

        }

        //Post for Edit Method
        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel productModel, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image == null)
                {
                    //var fil = _dbContext.Products.FirstOrDefault(c => c.Image == productModel.Image);
                    //var fil =await _dbContext.Products.FindAsync(productModel.Id);
                    var fil = _cloneRepository.Clone(productModel);
                    productModel.Image = fil.Image;

                    if (productModel.Image == null)
                    {
                        productModel.Image = "Images/profile.png";
                    }
                    // _dbContext.Dispose();
                    //_dbContext?.Dispose();
                    //_dbContext = new ApplicationDbContext(_dbContext.optio);
                }

                if (image != null)
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    productModel.Image = "Images/" + image.FileName;
                }
                 _dbContext.Products.Update(productModel);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(productModel);
        }



        //Get for Details Method
        public IActionResult Details(int? Id)
        {
            ViewData["productTypeid"] = new SelectList(_dbContext.ProductTypes.ToList(), "Id", "ProductType");   //This is to populate the ListView from the Database
            ViewData["specialTagid"] = new SelectList(_dbContext.SpecialTags.ToList(), "Id", "SpecialTag");

            if (Id==null)
            {
                return NotFound();
            }
            else
            {
                //var result = _dbContext.Products.Find(Id);
                var result = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).FirstOrDefault(f => f.Id == Id);

                return View(result);
            }
            
        }

        //Post for Details Method
        [HttpPost]
        public async Task<IActionResult> Details(ProductModel productModel, IFormFile image)
        {

            return View(productModel);
        }




        //Get for Delete Method
        public IActionResult Delete(int? Id)
        {
           
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                //var result = _dbContext.Products.Find(Id);
                var result = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).FirstOrDefault(f => f.Id == Id);

                return View(result);
            }

        }

        //Post for Details Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfrimDelete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            else
            {
                //var result = _dbContext.Products.Find(Id);
                var result = _dbContext.Products.Include(c => c.productTypes).Include(g => g.TagTypes).FirstOrDefault(f => f.Id == id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {

                    _dbContext.Products.Remove(result);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
            }
        }
    }
}
