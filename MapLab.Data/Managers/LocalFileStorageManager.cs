﻿using MapLab.Data.Managers.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.Text;

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
            => await SaveFileAsync(file.OpenReadStream(), tableName, propertyName, entityId, Path.GetExtension(file.FileName));

        public async Task<string> SaveFileAsync(IBrowserFile file, string tableName, string propertyName, string entityId)
            => await SaveFileAsync(file.OpenReadStream(), tableName, propertyName, entityId, Path.GetExtension(file.Name));

        public async Task<string> SaveFileAsync(string base64String, string tableName, string propertyName, string entityId, string fileExtension)
            => await SaveFileAsync(new MemoryStream(Convert.FromBase64String(base64String)), tableName, propertyName, entityId, fileExtension);

        public async Task<string> SaveJsonFileAsync(string json, string tableName, string propertyName, string entityId)
            => await SaveFileAsync(new MemoryStream(Encoding.UTF8.GetBytes(json)), tableName, propertyName, entityId, ".json");

        public async Task<string> SaveFileAsync(Stream fileStream, string tableName, string propertyName, string entityId, string fileExtension)
        {
            if (fileStream == null)
                return null!;

            var folderPath = Path.Combine(_storagePath, tableName, propertyName);
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, entityId + fileExtension);

            if (File.Exists(filePath))
                File.Delete(filePath);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            var relativePath = filePath.Replace("wwwroot", string.Empty);
            return relativePath;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            filePath = $"wwwroot{filePath}";

            try
            {
                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}