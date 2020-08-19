using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Shop.Client.Models;
using Shop.Shared.Models;

// Implements interface methods for model manipulation

namespace Shop.Client.Services
{
    public class MockOrdersDataService : IOrdersDataService
    {
        private MockShopDbContext _context { get; set; }
        private HttpResponseMessage res { get; set; }

        public MockOrdersDataService(MockShopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            res = new HttpResponseMessage();
        }

        public async Task<OrderDto> GetOrder(string name)
        {
            await _context.GetProductsAsync();
            await _context.GetOrderAsync();

            _context.order.Email = name;

            return _context.order;
        }

        public async Task<HttpResponseMessage> AddOrder(OrderChangeDto order)
        {
            var email = _context.order.Email;

            _context.order = new OrderDto()
            {
                Id = 1,
                Address = order.Address,
                Phone = order.Phone,
                Status = order.Status,
                Email = email,
                Time = DateTime.Now,
                OrderItems = new List<OrderItemDto>()
            };

            await _context.SetOrderAsync(_context.order);

            var jsonString = new StringContent(
                JsonSerializer.Serialize<OrderDto>(_context.order),
                Encoding.UTF8,
                "application/json");

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Content = jsonString
            };

            return res;
        }
        public async Task<HttpResponseMessage> UpdateOrder(int id, OrderChangeDto order)
        {
            _context.order.Address = order.Address;
            _context.order.Phone = order.Phone;
            _context.order.Status = order.Status;

            await _context.SetOrderAsync(_context.order);

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            return res;
        }

        public async Task<HttpResponseMessage> AddOrderItem(OrderItemChangeDto item)
        {
            var i = JsonSerializer.Deserialize<OrderItemDto>(
                    JsonSerializer.Serialize<OrderItemChangeDto>(item));

            var product = _context.products
                .Where(p => p.Id == item.ProductId)
                .FirstOrDefault();

            var existingItem = _context.order.OrderItems
                .Find(i => i.ProductId == item.ProductId);

            i.Id = _context.order.OrderItems.Count + 1;
            i.Price = item.Amount * product.Price;
            i.Product = product;

            // If the item exists in the order, correct the amount and price
            if (existingItem != null)
            {
                existingItem.Price += i.Price;
                existingItem.Amount += i.Amount;
            }
            // Otherwise, add the new item
            else
            {
                _context.order.OrderItems.Add(i);
            }

            CalculateTotal(_context.order);

            await _context.SetOrderAsync(_context.order);

            var jsonString = new StringContent(
                JsonSerializer.Serialize<OrderDto>(_context.order),
                Encoding.UTF8,
                "application/json");

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Content = jsonString
            };

            return res;
        }

        public async Task<HttpResponseMessage> UpdateOrderItem(int id, OrderItemChangeDto item)
        {
            var i = _context.order.OrderItems
                .Where(o => o.ProductId == item.ProductId)
                .FirstOrDefault();

            i.Amount = item.Amount;
            i.Price = item.Amount * i.Product.Price;

            CalculateTotal(_context.order);

            await _context.SetOrderAsync(_context.order);

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            return res;
        }

        public async Task<HttpResponseMessage> DeleteOrderItem(int id)
        {
            var item = _context.order.OrderItems.Where(p => p.Id == id).FirstOrDefault();

            _context.order.Total -= item.Price;
            _context.order.OrderItems.Remove(item);

            await _context.SetOrderAsync(_context.order);

            res = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            return res;
        }

        // Calculate the total
        private static void CalculateTotal(OrderDto order)
        {
            order.Total = 0;
            foreach (var item in order.OrderItems)
                order.Total += item.Price;
        }
    }
}
