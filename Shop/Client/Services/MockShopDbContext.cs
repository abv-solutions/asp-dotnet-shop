using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Shop.Shared.Models;

namespace Shop.Client.Services
{
    public class MockShopDbContext
    {
        private ILocalStorageService _localStorage { get; set; }
        public List<ProductDto> products { get; set; }
        public OrderDto order { get; set; }

        public MockShopDbContext(ILocalStorageService localStorage)
        {
            products = new List<ProductDto>()
            {
                new ProductDto
                {
                    Id = 1,
                    Name = "Apple",
                    Price = 12.95M,
                    Description = "Our famous apple!",
                    InStock = true,
                    Favourite = true
                },
                new ProductDto
                {
                    Id = 2,
                    Name = "Pear",
                    Price = 9.95M,
                    Description = "Our famous pear!",
                    InStock = false,
                    Favourite = false
                },
                new ProductDto
                {
                    Id = 3,
                    Name = "Cheese",
                    Price = 15.95M,
                    Description = "Our famous cheese!",
                    InStock = true,
                    Favourite = true
                },
                new ProductDto
                {
                    Id = 4,
                    Name = "Meat",
                    Price = 21.95M,
                    Description = "Our famous meat!",
                    InStock = false,
                    Favourite = true
                },
                new ProductDto
                {
                    Id = 5,
                    Name = "Blueberry",
                    Price = 5.95M,
                    Description = "Our famous blueberry!",
                    InStock = true,
                    Favourite = false
                },
                new ProductDto
                {
                    Id = 6,
                    Name = "Bread",
                    Price = 18.95M,
                    Description = "Our famous bread!",
                    InStock = false,
                    Favourite = true
                }
            };
            order = new OrderDto()
            {
                Id = 1,
                Address = "-",
                Phone = "-",
                Status = "open",
                Time = DateTime.Now,
                OrderItems = new List<OrderItemDto>()
            };

            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }

        public async Task GetProductsAsync()
        {
            var storageProducts = await _localStorage.GetItemAsync<List<ProductDto>>("products");

            if (storageProducts == null)
                await SetProductsAsync(products);
            else
                products = storageProducts;
        }

        public async Task SetProductsAsync(List<ProductDto> products)
        {
            await _localStorage.SetItemAsync("products", products);
        }

        public async Task GetOrderAsync()
        {
            var storageOrder = await _localStorage.GetItemAsync<OrderDto>("order");

            if (storageOrder == null)
                await SetOrderAsync(order);
            else
                order = storageOrder;
        }

        public async Task SetOrderAsync(OrderDto order)
        {
            await _localStorage.SetItemAsync("order", order);
        }
    }
}
