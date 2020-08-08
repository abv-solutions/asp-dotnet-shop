// Maps API view

namespace Shop.Shared.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public bool Favourite { get; set; }
    }
}
