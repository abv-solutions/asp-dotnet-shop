﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Maps DB table

namespace Shop.Server.Entities
{
    public class Order
    {        
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        [Required]
        [MaxLength(25)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime Time { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

}
