using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shop.Server.Entities;
using Microsoft.EntityFrameworkCore.Internal;

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
                .Where(o => o.Email == email && o.Status == "open")
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.Time = DateTime.Now;
            await CalculatePrices(order);

            if (order.Status == "open")
                await CloseOrders(order);

            _context.Orders.Add(order);
        }

        public async Task UpdateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.Time = DateTime.Now;
            await CalculatePrices(order);
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

        // Calculate the prices and total
        private async Task CalculatePrices(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId)
                    ?? throw new DbUpdateException("One or more of the Product IDs provided are invalid");
                if (!product.InStock)
                    throw new DbUpdateException("One or more of the Products are no longer in stock");

                item.Price = item.Amount * product.Price;
                order.Total += item.Price;
            }
        }

        // Close any previously opened order for current user
        private async Task CloseOrders(Order order)
        {
            var orders = await _context.Orders
                .Where(o => o.Email == order.Email && o.Status == "open")
                .ToListAsync();

            if (orders.Any())
                foreach (var o in orders)
                    o.Status = "closed";
        }
    }
}
