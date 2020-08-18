using System;
using System.Collections.Generic;
using Shop.Shared.Models;

namespace Shop.Client.Services
{
    public class MockShopDbContext
    {
        public List<ProductDto> products { get; set; }
        public OrderDto order { get; set; }

        public MockShopDbContext()
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
        }
    }
}
