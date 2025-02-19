using Microsoft.AspNetCore.Http;

namespace MapLab.Data.Managers.Contracts
{
    public interface IFileStorageManager
    {
        public Task<byte[]?> GetFileAsync(string? filePath);
        public Task<string> SaveFileAsync(IFormFile file, string tableName, string propertyName, string entityId);
    }
}
