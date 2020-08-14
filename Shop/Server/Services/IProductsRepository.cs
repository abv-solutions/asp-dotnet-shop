using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Server.Entities;
using Shop.Server.Resources;

// Defines methods for DB manipulation

namespace Shop.Server.Services
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProducts(ProductRouteParams resources);
        Task<Product> GetProduct(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<bool> Save();
    }
}
