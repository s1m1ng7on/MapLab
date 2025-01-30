using MapLab.Data.Managers.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLab.Data.Managers
{
    public class AzureBlobFileStorageManager : IFileStorageManager
    {
        public Task<byte[]> GetFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFileAsync(IFormFile file, string entityName, string entityId, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
