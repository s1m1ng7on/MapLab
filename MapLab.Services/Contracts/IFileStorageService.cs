using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface IFileStorageService
    {
        public Task<byte[]> GetFileAsync(string filePath);
        public Task<string> SaveFileAsync(IFormFile file, string entityName, string entityId, string propertyName);
    }
}
