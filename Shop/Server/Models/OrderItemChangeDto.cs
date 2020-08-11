using Shop.Server.Resources;
using Shop.Shared.Models;

// Maps API view

namespace Shop.Server.Models
{
    public class OrderItemChangeDto
    {
        public int Amount { get; set; }
        [ShopReadOnly]
        public decimal? Price { get; set; }
        public int OrderId { get; set; }
        public ProductDto Product { get; set; }
        public int ProductId { get; set; }
    }
}
