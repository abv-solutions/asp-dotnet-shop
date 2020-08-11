using System.ComponentModel.DataAnnotations;
using Shop.Server.Models;

// Custom field validation

namespace Shop.Server.Resources
{
    public class ShopReadOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
                return new ValidationResult(
                    $"The field '{validationContext.MemberName}' is read-only",
                    new[] { validationContext.MemberName });

            return ValidationResult.Success;
        }
    }
}
