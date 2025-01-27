using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Common.Attributes.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public FileExtensionAttribute(params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                string fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                return Array.Exists(_allowedExtensions, e => e.Equals(fileExtension));
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The file must have one of the following extensions: {string.Join(", ", _allowedExtensions)}.";
        }
    }
}
