using AutoMapper;
using Shop.Shared.Models;
using Shop.Server.Entities;
using Shop.Server.Models;

// Defines mappings between models and/or entities

namespace Shop.Server.Profiles
{
    public class OrdersProfile: Profile
    {
        public OrdersProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderChangeDto, Order>();
            CreateMap<OrderItemChangeDto, OrderItem>();
        }
    }
}
