using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var Users = _userManager.GetUsersInRoleAsync("User").Result;
            var result = _dbContext.ApplicationUsers.ToList();

            return View(Users);
        }



        //Get Details Method
        public IActionResult Details(string id)
        {
            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }




        //Get Disable Method
        public IActionResult Disable(string id)
        {
            var result = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        //Post Disable Method
        [HttpPost]
        [ActionName("Disable")]
        public async Task<IActionResult> DisableUser(string id)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            user.LockoutEnd = DateTime.Now.AddYears(100);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        //Get Activate Method
        public IActionResult Activate(string id)
        {
            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        //Post Activate Method
        [HttpPost]
        [ActionName("Activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            user.LockoutEnd = null;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



 //Get Delete Method
        public IActionResult Delete(string id)
        {
            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }


        //Post Delete Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            _dbContext.ApplicationUsers.Remove(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
