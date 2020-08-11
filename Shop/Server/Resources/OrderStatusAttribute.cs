using System.ComponentModel.DataAnnotations;
using Shop.Server.Models;

// Custom field validation

namespace Shop.Server.Resources
{
    public class StatusAttribute : ValidationAttribute
    {
        public StatusAttribute()
        {
            ErrorMessage = "The order field 'Status' must be one of the following: 'open', 'closed'";
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value?.ToString().ToLower() != "open" && value?.ToString().ToLower() != "closed")
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(OrderChangeDto.Status) });

            return ValidationResult.Success;
        }
    }
}
