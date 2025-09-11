using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Validation
{
    public class MaxSizeMB : ValidationAttribute
    {
        private readonly int size;

        public MaxSizeMB(int size)
        {
            this.size = size;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > size * 1024 * 1024)
                {
                    return new ValidationResult($"Max allowed size is {size} MB");
                }
            }
            return ValidationResult.Success;
        }
    }
}
