using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shop.Client.Models;
using Shop.Client.Resources;
using Shop.Shared.Models;

// Implements interface methods for model manipulation

namespace Shop.Client.Services
{
    public class MockProductsDataService : IProductsDataService
    {
        private MockShopDbContext _context { get; set; }
        private HttpResponseMessage res { get; set; }

        public MockProductsDataService(MockShopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            res = new HttpResponseMessage();
        }

        public Task<IEnumerable<ProductDto>> GetProducts()
        {
            return Task.FromResult<IEnumerable<ProductDto>>(_context.products);
        }

        public Task<IEnumerable<ProductDto>> GetProducts(ProductRouteParams p)
        {
            var query = _context.products.AsQueryable();

            foreach (PropertyInfo prop in p.GetType().GetProperties())
            {
                var val = prop.GetValue(p);
                var name = prop.Name;

                if (val != null)
                    query = (name == "InStock" || name == "Favourite")
                        ? query.Where(name + " == @0", val)
                        : query.Where(name + ".Contains(@0)", val);
            }
            
            return Task.FromResult<IEnumerable<ProductDto>>
                (query.OrderBy(p => p.Name));
        }

        public Task<ProductChangeDto> GetProduct(int id)
        {
            var product = _context.products.Where(p => p.Id == id).FirstOrDefault();

            return Task.FromResult<ProductChangeDto>
                (JsonSerializer.Deserialize<ProductChangeDto>(
                    JsonSerializer.Serialize<ProductDto>(product)));
        }

        public Task<HttpResponseMessage> AddProduct(ProductChangeDto product)
        {
            var p = JsonSerializer.Deserialize<ProductDto>(
                    JsonSerializer.Serialize<ProductChangeDto>(product));

            p.Id = _context.products.Count + 1;

            _context.products.Add(p);

            var jsonString = new StringContent(
                JsonSerializer.Serialize<ProductDto>(p),
                Encoding.UTF8,
                "application/json");

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Content = jsonString
            };

            return Task.FromResult<HttpResponseMessage>(res);
        }

        public Task<HttpResponseMessage> UpdateProduct(int id, ProductChangeDto product)
        {
            var p = _context.products.Where(p => p.Id == id).FirstOrDefault();

            p.Name = product.Name;
            p.Description = product.Description;
            p.Price = product.Price;
            p.InStock = product.InStock;

            var item = _context.order.OrderItems
                        .Where(o => o.ProductId == p.Id)
                        .FirstOrDefault();

            if (item != null)
            {
                var newPrice = item.Amount * product.Price;
                _context.order.Total = _context.order.Total - item.Price + newPrice;
                item.Price = newPrice;
            }

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            return Task.FromResult<HttpResponseMessage>(res);
        }

        public Task<HttpResponseMessage> DeleteProduct(int id)
        {
            _context.products = _context.products.Where(p => p.Id != id).ToList();

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            return Task.FromResult<HttpResponseMessage>(res);
        }
    }
}
