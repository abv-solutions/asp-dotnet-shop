using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shop.Server.Entities;

// Implements interface methods for DB manipulation

namespace Shop.Server.Services
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ShopDbContext _context;

        public OrdersRepository(ShopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Order>> GetOrders(string email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            return await _context.Orders
                .Where(o => o.Email == email)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.Time = DateTime.Now;

            foreach (var o in order.OrderItems)
            {
                o.Product = _context.Products.Find(o.ProductId)
                    ?? throw new DbUpdateException("One or more of the Product IDs provided are invalid");
                o.Price = o.Amount * o.Product.Price;
                order.Total += o.Price;
            }

            _context.Orders.Add(order);
        }
        public async Task<bool> Save()
        {
            try
            {
                return await _context.SaveChangesAsync() >= 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
