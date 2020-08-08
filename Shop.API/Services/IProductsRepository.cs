using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.API.Entities;
using Shop.API.Resources;

// Defines methods for DB manipulation

namespace Shop.API.Services
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProducts(ProductsRouteParams resources);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<Product> GetProduct(int id);
        Task<bool> Save();
    }
}
