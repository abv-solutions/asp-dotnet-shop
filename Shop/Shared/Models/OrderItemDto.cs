// Maps API view

namespace Shop.Shared.Models
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public ProductDto Product { get; set; }
        public int ProductId { get; set; }
    }
}
