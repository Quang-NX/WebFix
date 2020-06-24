﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class EcommerceDbContext:DbContext
    {
        public EcommerceDbContext() : base("EcommerceDbContext")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<EcommerceDbContext>());
            // Database.SetInitializer(new EcommerceDbContextSeed());    
            Database.SetInitializer(new UniDBInitializer<EcommerceDbContext>());
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
