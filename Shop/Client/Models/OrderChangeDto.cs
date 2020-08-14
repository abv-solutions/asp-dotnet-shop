using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Shared.Models;

// Maps client view - for creation

namespace Shop.Client.Models
{
    public class OrderChangeDto
    {
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        [Required]
        [MaxLength(25)]
        public string Phone { get; set; }
        [Required]
        public string Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
