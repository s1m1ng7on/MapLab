using MapLab.Data.Managers.Contracts;
using Microsoft.AspNetCore.Http;

namespace MapLab.Data.Managers
{
    public class LocalFileStorageManager : IFileStorageManager
    {
        private readonly string _storagePath = "wwwroot/uploads";

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string entityName, string entityId, string propertyName)
        {
            if (file == null || file.Length == 0)
                return null;

            var folderPath = Path.Combine(_storagePath, entityName, entityId);
            Directory.CreateDirectory(folderPath);

            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, propertyName + fileExtension);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
        }
    }
}
