using System.ComponentModel.DataAnnotations;
using Shop.Server.Resources;

// Maps API view - for creation

namespace Shop.Server.Models
{
    public class ProductChangeDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public decimal Price { get; set; }        
        public bool InStock { get; set; }
        [ShopReadOnly]
        public bool? Favourite { get; set; }
    }
}
