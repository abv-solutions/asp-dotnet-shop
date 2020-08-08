using System.ComponentModel.DataAnnotations;

// Maps DB table

namespace Shop.Server.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public bool Favourite { get; set; }

    }
}
