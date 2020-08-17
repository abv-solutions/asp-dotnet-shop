using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Server.Entities;

// Defines methods for DB manipulation

namespace Shop.Server.Services
{
    public interface IOrderItemsRepository
    {
        Task<OrderItem> GetOrderItem(int id);
        Task<Order> AddOrderItem(OrderItem item);
        Task UpdateOrderItem(OrderItem item);
        Task<bool> Save();
    }
}
