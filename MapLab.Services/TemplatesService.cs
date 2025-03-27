using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;
        private readonly IMapper _mapper;

        public TemplatesService(IDeletableEntityRepository<MapTemplate> mapTemplateRepository)
        {
            _mapTemplateRepository = mapTemplateRepository;
        }

        public async Task<MapTemplateDto> GetMapTemplateAsync(string id)
        {
            var newsArticle = await _mapTemplateRepository.All()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<MapTemplateDto>(newsArticle);
        }
    }
}
