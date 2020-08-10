using System;
using System.Collections.Generic;

// Maps DB table

namespace Shop.Server.Entities
{
    public class Order
    {        
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public DateTime Time { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

}
