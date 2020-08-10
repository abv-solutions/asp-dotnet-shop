using IdentityServer4.EntityFramework.Options;
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
                .Entity<Product>()
                .HasData(new Product
                {
                    Id = 1,
                    Name = "Apple",
                    Price = 12.95M,
                    Description = "Our famous apple!",
                    InStock = true,
                    Favourite = false
                },
                new Product
                {
                    Id = 2,
                    Name = "Pear",
                    Price = 9.95M,
                    Description = "Our famous pear!",
                    InStock = false,
                    Favourite = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Cheese",
                    Price = 15.95M,
                    Description = "Our famous cheese!",
                    InStock = true,
                    Favourite = false
                });

            modelBuilder
                .Entity<Order>()
                .HasData(new Order()
                {
                    Id = 1,
                    Address = "dummy address",
                    Phone = "0040555444",
                    Email = "andrei@gmail.com",
                    Total = 22.9M,
                    Time = DateTime.Now
                });

            modelBuilder
                .Entity<OrderItem>()
                .HasData(new OrderItem()
                {
                    Id = 1,
                    OrderId = 1,
                    Amount = 2,
                    Price = 12.95M,
                    ProductId = 1
                },
                new OrderItem()
                {
                    Id = 2,
                    OrderId = 1,
                    Amount = 1,
                    Price = 9.95M,
                    ProductId = 2
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
