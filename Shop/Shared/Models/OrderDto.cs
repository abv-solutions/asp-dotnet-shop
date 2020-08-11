using System;
using System.Collections.Generic;

// Maps API view

namespace Shop.Shared.Models
{
    public class OrderDto
    {        
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime Time { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

}
