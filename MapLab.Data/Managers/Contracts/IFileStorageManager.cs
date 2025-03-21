using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace MapLab.Data.Managers.Contracts
{
    public interface IFileStorageManager
    {
        public Task<byte[]?> GetFileAsync(string? filePath);
        public Task<string> SaveFileAsync(IFormFile file, string tableName, string propertyName, string entityId);
        public Task<string> SaveFileAsync(IBrowserFile file, string tableName, string propertyName, string entityId);
        public Task<string> SaveFileAsync(string base64String, string tableName, string propertyName, string entityId, string fileExtension);
        public Task<string> SaveJsonFileAsync(string json, string tableName, string propertyName, string entityId);
        public Task<string> SaveFileAsync(Stream fileStream, string tableName, string propertyName, string entityId, string fileExtension);
        public Task<bool> DeleteFileAsync(string filePath);
    }
}
