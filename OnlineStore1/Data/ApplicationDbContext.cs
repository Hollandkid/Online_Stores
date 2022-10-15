using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<SpecialTagModel> SpecialTags { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<AdminUserModel> AdminUsers { get; set; }

        
    }
}
