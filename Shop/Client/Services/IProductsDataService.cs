using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shop.Client.Models;
using Shop.Shared.Models;

// Defines methods for model manipulation

namespace Shop.Client.Services
{
    public interface IProductsDataService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductChangeDto> GetProduct(int id);
        Task<HttpResponseMessage> AddProduct(ProductChangeDto product);
        Task<HttpResponseMessage> UpdateProduct(int id, ProductChangeDto product);
        Task<HttpResponseMessage> DeleteProduct(int id);
    }
}
