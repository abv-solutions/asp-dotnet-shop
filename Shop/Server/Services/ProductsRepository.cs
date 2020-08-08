using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Shop.Server.Entities;
using Shop.Server.Resources;
using System.Reflection;

// Implements interface methods for DB manipulation

namespace Shop.Server.Services
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ShopDbContext _context;

        public ProductsRepository(ShopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(ProductsRouteParams resources)
        {
            bool search = false;
            var query = _context.Products as IQueryable<Product>;

            foreach (PropertyInfo prop in resources.GetType().GetProperties())
            {
                var val = prop.GetValue(resources);
                var name = prop.Name;

                if (val != null)
                {
                    search = true;
                    query = (name == "InStock" || name == "Favourite")
                        ? query.Where(name + " == @0", val)
                        : query.Where(name + ".Contains(@0)", val);
                }
            }

            if (search)
                return await query
                    .OrderBy(p => p.Name)
                    .AsNoTracking()
                    .ToListAsync();

            return await GetProducts();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _context.Products.Remove(product);
        }

        public async Task<bool> Save()
        {
            try
            {
                return await _context.SaveChangesAsync() >= 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
