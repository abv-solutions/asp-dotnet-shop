﻿using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

// Defines DB context

namespace Shop.Server.Entities
{
    public class ShopDbContext : ApiAuthorizationDbContext<ShopUser>
    {
        public ShopDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>(entity => entity.HasCheckConstraint(
                    "CK_Order_Status",
                    "[Status] IN ('open', 'closed')"));

            modelBuilder
                .Entity<Product>()
                .HasData(new Product
                {
                    Id = 1,
                    Name = "Apple",
                    Price = 12.95M,
                    Description = "Our famous apple!",
                    InStock = true,
                    Favourite = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Pear",
                    Price = 9.95M,
                    Description = "Our famous pear!",
                    InStock = false,
                    Favourite = false
                },
                new Product
                {
                    Id = 3,
                    Name = "Cheese",
                    Price = 15.95M,
                    Description = "Our famous cheese!",
                    InStock = true,
                    Favourite = true
                },
                new Product
                {
                    Id = 4,
                    Name = "Meat",
                    Price = 21.95M,
                    Description = "Our famous meat!",
                    InStock = false,
                    Favourite = true
                },
                new Product
                {
                    Id = 5,
                    Name = "Blueberry",
                    Price = 5.95M,
                    Description = "Our famous blueberry!",
                    InStock = true,
                    Favourite = false
                },
                new Product
                {
                    Id = 6,
                    Name = "Bread",
                    Price = 18.95M,
                    Description = "Our famous bread!",
                    InStock = false,
                    Favourite = true
                });


            base.OnModelCreating(modelBuilder);
        }
    }
}
