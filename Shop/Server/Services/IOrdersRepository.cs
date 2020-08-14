using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Server.Entities;

// Defines methods for DB manipulation

namespace Shop.Server.Services
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrders(string email);
        Task<Order> GetOrder(int id);
        Task AddOrder(Order order);
        Task<bool> Save();
    }
}
