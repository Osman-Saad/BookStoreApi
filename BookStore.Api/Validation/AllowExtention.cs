using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Validation
{
    public class AllowExtention : ValidationAttribute
    {
        private readonly string[] extentions;

        public AllowExtention(string[] extentions)
        {
            this.extentions = extentions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var fileExtention = Path.GetExtension(file.FileName);
                if (!extentions.Contains(fileExtention.ToLower()))
                {
                    return new ValidationResult($"This extention {fileExtention} is not allowed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
