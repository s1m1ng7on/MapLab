using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Common.Attributes.Validation
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;
        public MaxFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                if (file.Length < _maxSize)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
