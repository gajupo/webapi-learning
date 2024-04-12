using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace webapi_learning.Models.Validations
{
    public class Shirt_EnsureTheCorrectSizingAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Shirt? shirt = validationContext.ObjectInstance as Shirt;

            if (shirt != null && !string.IsNullOrEmpty(shirt.Gender))
            {
                if (shirt.Gender.Equals("men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 10)
                {
                    return new ValidationResult("For men's shirts, the size has be greaten than 10");
                } else if (shirt.Gender.Equals("women", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6) {
                    return new ValidationResult("For women's shirts, the size has to be grater than 6");
                }
            }

            return ValidationResult.Success;
        }
    }
}
