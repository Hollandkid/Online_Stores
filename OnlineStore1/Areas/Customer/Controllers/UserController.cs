using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore1.Data;
using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string email)
        {
            email = email ?? "null@null.com";
            var user = await _userManager.FindByEmailAsync(email);
            if (null == user)
            {
                return NotFound();
            }
            return View(user);
        }


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
            returnUrl = returnUrl ?? Url.Content("/Customer/Home/Index");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var isUser = await _userManager.IsInRoleAsync(user, "User");
            if (!isUser)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt, You are not a Customer \n Only Customers are allowed to Sign In.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check Login credentials");
                return View();
            }
            TempData["save"] = "Welcome User";
            return LocalRedirect(returnUrl);
        }


        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


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

                var chkuser = await _userManager.CreateAsync(newUser, user.Password);

                if (chkuser.Succeeded)
                {
                    var result = await _userManager.AddToRoleAsync(newUser, "User");
                    if (result.Succeeded)
                    {
                        //Email Authentication is Required

                        TempData["save"] = "You have successfully Register as a User";

                        var SignedIn = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
                        if (SignedIn.Succeeded)
                        {
                            TempData["save"] = "Welcome User";
                            //return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check Login credentials");
                            return View();
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in chkuser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(user);
        }



        public IActionResult Create()
        {
            return View();
        }

        //Post Create Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {

                //Add to Db

                var result = await _userManager.CreateAsync(applicationUser);

                if (result.Succeeded)
                {
                    TempData["save"] = "Account created Successfully";
                    //await _userManager.AddToRoleAsync(applicationUser, "User");       //This is to Assign a role to a User
                }
                //for Errors
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }



        //Get EditProfile Method
        public async Task<IActionResult> Edit(string Id)
        {
            //var editResult = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == Id);
            var user = await _userManager.FindByIdAsync(Id);

            if (null == user)
            {
                NotFound();
            }

            return View(user);
        }

        //Post EditProfile Method
        [HttpPost]
        //[ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser applicationUser)
        {
            if (null != applicationUser.Id)
            {
                //Edit/ Update User Info

                //var user = _dbContext.ApplicationUsers.FirstOrDefault(f => f.Id == applicationUser.Id);
                var user = await _userManager.FindByIdAsync(applicationUser.Id);

                if (null == user)
                {
                    return NotFound();
                }
                user.UserName = applicationUser.UserName;
                user.FirstName = applicationUser.FirstName;
                user.LastName = applicationUser.LastName;
                user.Address = applicationUser.Address;
                user.PhoneNumber = applicationUser.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["save"] = "Details Update Successfully";
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", new { email = user.Email });
                }
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }

            }

            return View(applicationUser);
        }



        //Get Detail Method....  This is to get the User details
        public IActionResult Details(string Id)
        {
            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == Id);

            if (userInfo == null)
            {
                NotFound();
            }

            return View(userInfo);
        }




        //Get Disable Method....  This is to Delete/LockOut a User 
        public IActionResult Disable(string Id)
        {

            if (Id == null)
            {
                return NotFound();
            }
            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == Id);

            if (userInfo == null)
            {
                NotFound();
            }
            return View(userInfo);
        }

        //Post Disable Method... This is to Delete/LockOut a User
        [HttpPost]
        public IActionResult Disable(ApplicationUser user)
        {

            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();

            }
            else
            {
                userInfo.LockoutEnd = DateTime.Now.AddYears(100);

                int rowAffected = _dbContext.SaveChanges();
                if (rowAffected > 0)
                {
                    TempData["save"] = "User Disabled";
                    return RedirectToAction("Index");
                }
            }


            return View(userInfo);
        }




        //Get Activate User Account Method...
        public IActionResult Activate(string id)
        {

            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            if (userInfo == null)
            {
                return NotFound();

            }

            return View(userInfo);
        }


        //Post Activate User Account Method...
        [HttpPost]
        public IActionResult Activate(ApplicationUser user)
        {

            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();

            }
            else
            {
                userInfo.LockoutEnd = null;     //this can also be used  ==>> userInfo.LockoutEnd = DateTime.Now.AddDays(-1);

                int rowAffected = _dbContext.SaveChanges();
                if (rowAffected > 0)
                {
                    TempData["save"] = "User Account Activated";
                    return RedirectToAction("Index");
                }
            }
            return View(userInfo);
        }




        //Get Delete User Account Method...
        public IActionResult Delete(string id)
        {

            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);

            if (userInfo == null)
            {
                return NotFound();

            }

            return View(userInfo);
        }

        //Post Delete User Account Method...
        [HttpPost]
        public IActionResult Delete(ApplicationUser user)
        {

            var userInfo = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();

            }
            else
            {

                _dbContext.ApplicationUsers.Remove(user);
                int rowAffected = _dbContext.SaveChanges();
                if (rowAffected > 0)
                {
                    TempData["save"] = "User Account Deleted";
                    return RedirectToAction("Index");
                }
            }
            return View(userInfo);
        }

    }
}
