using System.ComponentModel.DataAnnotations;

// Defines query parameters from API routes

namespace Shop.Server.Resources
{
    public class ProductsRouteParams
    {
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public bool? InStock { get; set; }
        public bool? Favourite { get; set; }
    }
}