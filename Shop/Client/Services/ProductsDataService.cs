using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shop.Client.Models;
using Shop.Shared.Models;

// Implements interface methods for model manipulation

namespace Shop.Client.Services
{
    public class ProductsDataService : IProductsDataService
    {
        private HttpClient _publicHttp { get; set; }
        private HttpClient _secureHttp { get; set; }

        public ProductsDataService(IHttpClientFactory HttpClientFactory, HttpClient secureHttp)
        {
            _publicHttp = HttpClientFactory.CreateClient("Shop.PublicAPI")
                ?? throw new ArgumentNullException(nameof(HttpClientFactory));
            _secureHttp = secureHttp ?? throw new ArgumentNullException(nameof(secureHttp));
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await _publicHttp.GetFromJsonAsync<IEnumerable<ProductDto>>("/api/products");
        }

        public async Task<ProductChangeDto> GetProduct(int id)
        {
            return await _publicHttp.GetFromJsonAsync<ProductChangeDto>($"/api/products/{id}");
        }

        public async Task<HttpResponseMessage> AddProduct(ProductChangeDto product)
        {
            product.Favourite = null;
            return await _secureHttp.PostAsJsonAsync("/api/products", product);
        }

        public async Task<HttpResponseMessage> UpdateProduct(int id, ProductChangeDto product)
        {
            product.Favourite = null;
            return await _secureHttp.PutAsJsonAsync($"/api/products/{id}", product);
        }

        public async Task<HttpResponseMessage> DeleteProduct(int id)
        {
            return await _secureHttp.DeleteAsync($"/api/products/{id}");
        }
    }
}
