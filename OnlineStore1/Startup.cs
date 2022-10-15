using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStore1.Areas.Admin.Controllers;
using OnlineStore1.Data;
using OnlineStore1.Models;
using OnlineStore1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        //This is to Create an Admin and Create the Roles
        private async Task CreateAdminandRoles(IServiceProvider serviceProvider)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var AdminRoleExist = await _roleManager.RoleExistsAsync("Admin");
            var UserRoleExist = await _roleManager.RoleExistsAsync("User");
            var AdminUserExist = await _userManager.FindByEmailAsync("Admin@Admin.com");

            if (!AdminRoleExist)
            {
                var role = new IdentityRole() { Name = "Admin" };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Admin Role not created");
                }
            }
            if (!UserRoleExist)
            {
                var role = new IdentityRole() { Name = "User" };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    Console.WriteLine("User Role not created");
                }
            }
            if (null == AdminUserExist)
            {
                var AdminUser = new ApplicationUser() { Email = "Admin@Admin.com", UserName = "Admin@Admin.com" };

                string AdminPassword = "123456";

                var newAdmin = await _userManager.CreateAsync(AdminUser, AdminPassword);
                if (newAdmin.Succeeded)
                {
                    await _userManager.AddToRoleAsync(AdminUser, "Admin");
                }
                else
                {
                    Console.WriteLine("Admin Account not created");
                }
            }

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric= false;
                options.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //services.AddDbContext<SchoolContext>(options => options.UseSqlServer(CONNECTION_STRING));

            services.AddScoped<CloneRepository, CloneRepository>();

            services.AddScoped<TestClass>();

            //This is to give change the Login Path for Redirection
            //services.ConfigureApplicationCookie(options => options.LoginPath = "/Admin/Admin/Login");
            services.ConfigureApplicationCookie(options => {
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        var requestPath = ctx.Request.Path;

                        if (requestPath == "/Customer/order/checkout")
                        {
                            ctx.Response.Redirect("/Customer/user/login");
                            options.LoginPath = "/Customer/user/login";
                        }
                        else
                        {
                            ctx.Response.Redirect("/Admin/Admin/Login");
                            options.LoginPath = "/Admin/Admin/Login";
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //this is for the App State and Session which was use in the Cart...
            services.AddSession(options =>
            {
                
                options.IdleTimeout = TimeSpan.FromMinutes(30);  //This is to set the Time
                // options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();       //This is for the App State and Session

            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateAdminandRoles(provider).Wait();
        }
    }
}
