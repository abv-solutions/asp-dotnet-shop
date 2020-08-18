using System.Net.Http;
using System.Threading.Tasks;
using Shop.Client.Models;
using Shop.Shared.Models;

// Defines methods for model manipulation

namespace Shop.Client.Services
{
    public interface IOrdersDataService
    {
        Task<OrderDto> GetOrder(string name);
        Task<HttpResponseMessage> AddOrder(OrderChangeDto order);
        Task<HttpResponseMessage> UpdateOrder(int id, OrderChangeDto order);
        Task<HttpResponseMessage> AddOrderItem(OrderItemChangeDto item);
        Task<HttpResponseMessage> UpdateOrderItem(int id, OrderItemChangeDto item);
        Task<HttpResponseMessage> DeleteOrderItem(int id);
    }
}
