using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore1.Data;
using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductTypesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {

            var data = _dbContext.ProductTypes.ToList();
            return View(data);
        }


        //Get Create Action Method
        public IActionResult Create()
        {

            
            return View();
        }


        //Post Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {

                //Add to Db
                _dbContext.ProductTypes.Add(productTypes);
                await _dbContext.SaveChangesAsync();
                TempData["save"] = "Product Type has been Saved";
                return RedirectToAction("Index");
            }

            return View(productTypes);
        }

        //Get Edit Action Method
        public IActionResult Edit(int? Id)
        {
            if (Id==null)
            {
                return NotFound();
            }

            var productType = _dbContext.ProductTypes.Find(Id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }


        //Post Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {

                //Uodate Db
                _dbContext.ProductTypes.Update(productTypes);
                await _dbContext.SaveChangesAsync();
                TempData["save"] = "Product Type Updated";

                return RedirectToAction("Index");
            }

            return View(productTypes);
        }


        //Get Details Action Method
        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var productType = _dbContext.ProductTypes.Find(Id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }


        //Post Details Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ProductTypes productTypes)
        {
            TempData["save"] = "";

            return RedirectToAction("Index");
        }


        //Get Delete Action Method
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var productType = _dbContext.ProductTypes.Find(Id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //Post Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id,ProductTypes productTypes)
        {
            if (Id==null && Id != productTypes.Id)
            {
                return NotFound();
            }
            var productType = _dbContext.ProductTypes.Find(Id);

            _dbContext.Remove(productType);
            await _dbContext.SaveChangesAsync();
            TempData["save"] = "Product Type Deleted";

            return RedirectToAction("Index");
        }
    }
}
