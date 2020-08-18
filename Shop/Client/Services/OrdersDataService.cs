using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using Shop.Client.Models;
using Shop.Shared.Models;

// Implements interface methods for model manipulation

namespace Shop.Client.Services
{
    public class OrdersDataService : IOrdersDataService
    {
        private HttpClient _secureHttp { get; set; }

        public OrdersDataService(HttpClient secureHttp)
        {
            _secureHttp = secureHttp ?? throw new ArgumentNullException(nameof(secureHttp));
        }

        public async Task<OrderDto> GetOrder(string name)
        {
            return (await _secureHttp.GetFromJsonAsync<IEnumerable<OrderDto>>($"/api/orders?email={name}"))
                .FirstOrDefault();
        }

        public async Task<HttpResponseMessage> AddOrder(OrderChangeDto order)
        {
            return await _secureHttp.PostAsJsonAsync("/api/orders", order);
        }
        public async Task<HttpResponseMessage> UpdateOrder(int id, OrderChangeDto order)
        {
            return await _secureHttp.PutAsJsonAsync($"/api/orders/{id}", order);
        }

        public async Task<HttpResponseMessage> AddOrderItem(OrderItemChangeDto orderItem)
        {
            return await _secureHttp.PostAsJsonAsync("/api/orderitems", orderItem);
        }

        public async Task<HttpResponseMessage> UpdateOrderItem(int id, OrderItemChangeDto orderItem)
        {
            return await _secureHttp.PutAsJsonAsync($"/api/orderitems/{id}", orderItem);
        }

        public async Task<HttpResponseMessage> DeleteOrderItem(int id)
        {
            return await _secureHttp.DeleteAsync($"/api/orderitems/{id}");
        }
    }
}
