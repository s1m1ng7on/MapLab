using AutoMapper;
using AutoMapper.QueryableExtensions;
using MapLab.Data.Entities;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Services
{
    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<NewsArticle> _newsArticleRepository;
        private readonly IMapper _mapper;
        private readonly IProfileService _profileService;

        public NewsService(IDeletableEntityRepository<NewsArticle> newsArticleRepository, IMapper mapper, IProfileService profileService)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
            _profileService = profileService;
        }

        public async Task<NewsPaginationDto> GetNewsAsync(int page, int pageSize)
        {
            var query = _newsArticleRepository.All()
                .Include(x => x.Profile)
                .OrderByDescending(x => x.CreatedOn);

            var totalCount = await query.CountAsync();

            var articles = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<NewsArticleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new NewsPaginationDto
            {
                Articles = articles,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize,
            };
        }

        public async Task<NewsArticleDto> GetNewsArticleAsync(string id)
        {
            var newsArticle = await _newsArticleRepository.All()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<NewsArticleDto>(newsArticle);
        }

        public async Task CreateNewsArticleAsync(NewsArticleDto newsArticleDto)
        {
            var newsArticle = new NewsArticle();
            _mapper.Map(newsArticleDto, newsArticle);
            newsArticle.ProfileId = _profileService.GetProfileId();

            await _newsArticleRepository.AddAsync(newsArticle);
            await _newsArticleRepository.SaveChangesAsync();
        }

        public async Task DeleteNewsArticleAsync(string id)
        {
            var newsArticle = await _newsArticleRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id);
            _newsArticleRepository.Delete(newsArticle);
            await _newsArticleRepository.SaveChangesAsync();
        }
    }
}
