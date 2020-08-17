using System.ComponentModel.DataAnnotations;

// Defines query parameters from API routes

namespace Shop.Client.Resources
{
    public class ProductRouteParams
    {
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public bool? InStock { get; set; }
        public bool? Favourite { get; set; }
    }
}