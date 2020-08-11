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
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<Product> GetProduct(int id);
        Task<bool> Save();
    }
}
