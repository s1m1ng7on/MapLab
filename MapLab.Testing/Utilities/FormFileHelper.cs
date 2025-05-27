using Microsoft.AspNetCore.Http;

namespace MapLab.Testing.Utilities
{
    public static class FormFileHelper
    {
        public static IFormFile Create(string fileName, string contentType, byte[] content)
        {
            var stream = new MemoryStream(content);
            return new FormFile(stream, 0, content.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
