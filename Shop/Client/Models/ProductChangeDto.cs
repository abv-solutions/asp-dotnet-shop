using System.ComponentModel.DataAnnotations;

// Maps client view - for creation

namespace Shop.Client.Models
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
        public bool? Favourite { get; set; }
    }
}
