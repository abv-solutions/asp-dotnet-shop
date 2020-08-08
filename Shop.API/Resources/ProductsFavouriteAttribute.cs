using System.ComponentModel.DataAnnotations;
using Shop.API.Models;

// Custom field validation

namespace Shop.API.Resources
{
    public class FavouriteAttribute : ValidationAttribute
    {
        public FavouriteAttribute()
        {
            ErrorMessage = "The product field 'Favourite' is read-only";
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(ProductChangeDto.Favourite) });

            return ValidationResult.Success;
        }
    }
}
