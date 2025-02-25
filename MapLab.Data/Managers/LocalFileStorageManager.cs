using MapLab.Data.Managers.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace MapLab.Data.Managers
{
    public class LocalFileStorageManager : IFileStorageManager
    {
        private readonly string _storagePath = "wwwroot/uploads";

        public async Task<byte[]?> GetFileAsync(string? filePath)
        {
            filePath = $"wwwroot{filePath}";

            if (!File.Exists(filePath))
                return null;

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string tableName, string propertyName, string entityId)
            => await SaveFileAsync(file.OpenReadStream(), file.Name, tableName, propertyName, entityId);

        public async Task<string> SaveFileAsync(IBrowserFile file, string tableName, string propertyName, string entityId)
            => await SaveFileAsync(file.OpenReadStream(), file.Name, tableName, propertyName, entityId);

        private async Task<string> SaveFileAsync(Stream fileStream, string fileName, string tableName, string propertyName, string entityId)
        {
            if (fileStream == null)
                return null!;

            var folderPath = Path.Combine(_storagePath, tableName, propertyName);
            Directory.CreateDirectory(folderPath);

            var fileExtension = Path.GetExtension(fileName);
            var filePath = Path.Combine(folderPath, entityId + fileExtension);

            if (File.Exists(filePath))
                File.Delete(filePath);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            var relativePath = filePath.Replace("wwwroot", string.Empty);
            return relativePath;
        }
    }
}
