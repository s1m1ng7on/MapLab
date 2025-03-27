using MapLab.Services.Models;

namespace MapLab.Services.Contracts
{
    public interface ITemplatesService
    {
        public Task<MapTemplateDto> GetMapTemplateAsync(string id);
    }
}
