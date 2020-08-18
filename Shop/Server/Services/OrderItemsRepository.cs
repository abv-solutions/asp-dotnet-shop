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
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ShopDbContext _context;

        public OrderItemsRepository(ShopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<OrderItem> GetOrderItem(int id)
        {
            return await _context.OrderItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var order = await CheckOrder(item);

            if (order.OrderItems == null)
                order.OrderItems = new List<OrderItem>();

            var existingItem = order.OrderItems
                .Find(i => i.ProductId == item.ProductId);

            item.Price = item.Amount * item.Product.Price;

            // If the item exists in the order, correct the amount and price
            if (existingItem != null)
            {
                existingItem.Price += item.Price;
                existingItem.Amount += item.Amount;
            }
            // Otherwise, add the new item
            else
            {
                order.OrderItems.Add(item);
                _context.OrderItems.Add(item);
            }

            CalculateTotal(order);

            return order;
        }

        public async Task UpdateOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var order = await CheckOrder(item);

            item.Price = item.Amount * item.Product.Price;

            CalculateTotal(order);
        }

        public void DeleteOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var order = _context.Orders.FirstOrDefault(o => o.Id == item.OrderId)
                ?? throw new DbUpdateException("The provided Order ID is invalid");

            order.Total -= item.Price;

            _context.OrderItems.Remove(item);
        }

        // Check order and item info
        private async Task<Order> CheckOrder(OrderItem item)
        {
            item.Product = await _context.Products.FindAsync(item.ProductId)
                ?? throw new DbUpdateException("One or more of the Product IDs provided are invalid");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == item.OrderId)
                ?? throw new DbUpdateException("The provided Order ID is invalid");

            if (order.Status == "closed")
                throw new DbUpdateException("This order is closed");

            return order;
        }

        // Calculate the total
        private static void CalculateTotal(Order order)
        {
            order.Total = 0;
            foreach (var item in order.OrderItems)
                order.Total += item.Price;
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
