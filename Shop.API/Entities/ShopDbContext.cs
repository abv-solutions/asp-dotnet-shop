using Microsoft.EntityFrameworkCore;

// Defines DB context

namespace Shop.API.Entities
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
