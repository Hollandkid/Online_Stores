using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore1.Data;
using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        //[Authorize(Roles = "Admin")]
        //Get Index Method
        public IActionResult Index()
        {
            var AdminAcctList = _userManager.GetUsersInRoleAsync("Admin").Result;
            return View(AdminAcctList);
        }




        //Get Register Action Method
        public IActionResult Register()
        {
            return View();
        }

        //Post Register Action Method
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {

                //Add to Db
                var newUser = new ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber
                };
                
                var chkuser = await _userManager.CreateAsync(newUser,user.Password);

                if (chkuser.Succeeded)
                {
                    var result = await _userManager.AddToRoleAsync(newUser, "Admin");
                    if (result.Succeeded)
                    {
                        TempData["save"] = "You have successfully Register as an Admin";
                    }
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                foreach (var error in chkuser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(user);
        }



        //Get Register Action Method
        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        //asp-route-email="@Model.Email" asp-route-password="@Model.Password"

        //Post Register Action Method
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/Admin/Admin/Index");

            if (ModelState.IsValid)
            {
                var Admin = await _userManager.FindByEmailAsync(model.Email);
                var isAdmin = await _userManager.IsInRoleAsync(Admin, "Admin");
                if (isAdmin)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        TempData["save"] = "You Logged In as an Admin";
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check Login credentials");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt, You are not an Admin \n Only Admins are allowed to Sign In here.");
                    return View();
                }
            }

            return View(model);
        }


        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
