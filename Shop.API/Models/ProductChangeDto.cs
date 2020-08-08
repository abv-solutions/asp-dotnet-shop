using System.ComponentModel.DataAnnotations;
using Shop.API.Resources;

// Maps API view - for creation

namespace Shop.API.Models
{
    public class ProductChangeDto
    {
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public decimal Price { get; set; }        
        public bool? InStock { get; set; }
        [Favourite]
        public bool? Favourite { get; set; }
    }
}
