using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Common.Models
{
    [NotMapped]
    public class SingleFile : FormFile
    {
        public string SavedFilePath { get; set; }

        public SingleFile(Stream baseStream, long baseStreamOffset, long length, string name, string fileName) : base(baseStream, baseStreamOffset, length, name, fileName)
        {
            string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data");

            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);

            string filePath = Path.Combine(dataPath, name);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                this.CopyToAsync(fileStream);
            }

            SavedFilePath = filePath;
        }
    }
}
