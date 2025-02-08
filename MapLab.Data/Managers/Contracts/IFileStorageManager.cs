using Microsoft.AspNetCore.Http;

namespace MapLab.Data.Managers.Contracts
{
    public interface IFileStorageManager
    {
        public Task<byte[]?> GetFileAsync(string? filePath);
        public Task<string> SaveFileAsync(IFormFile file, string entityName, string entityId, string propertyName);
    }

    public abstract class BaseFileStorageManager
    {
        public abstract string FileStorageCategory { get; set; }

        public abstract Task<string> SaveFileAsync(IFormFile file, string entityName, string entityId, string propertyName);
    }
}
