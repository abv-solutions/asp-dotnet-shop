using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Server.Resources;
using Shop.Shared.Models;

// Maps API view - for creation

namespace Shop.Server.Models
{
    public class OrderChangeDto
    {
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(25)]
        public string Phone { get; set; }
        [ShopReadOnly]
        public string Email { get; set; }
        [Required]
        [Status]
        public string Status { get; set; }
        [ShopReadOnly]
        public decimal? Total { get; set; }
        public DateTime Time { get; set; }
        public List<OrderItemChangeDto> OrderItems { get; set; }
    }
}
