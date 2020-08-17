using Shop.Shared.Models;

// Maps client view

namespace Shop.Client.Models
{
    public class OrderItemChangeDto
    {
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
