using AutoMapper;
using AutoMapper.QueryableExtensions;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MapLab.Services
{
    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<NewsArticle> _newsArticleRepository;
        private readonly IMapper _mapper;
        private readonly IProfileService _profileService;
        private readonly IFileStorageManager _fileStorageManager;

        public NewsService(IDeletableEntityRepository<NewsArticle> newsArticleRepository, IMapper mapper, IProfileService profileService, IFileStorageManager fileStorageManager)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
            _profileService = profileService;
            _fileStorageManager = fileStorageManager;
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
            newsArticle.ThumbnailFilePath = await _fileStorageManager.SaveFileAsync(newsArticleDto.Thumbnail, "News", "Thumbnails", newsArticle.Id);

            string base64Pattern = @"data:image\/(png|jpg|jpeg|gif);base64,([A-Za-z0-9+/=]+)";
            var matches = Regex.Matches(newsArticle.Content, base64Pattern);
            foreach (Match match in matches)
            {
                var matchString = match.ToString();
                var extension = "." + match.Groups[1].Value;
                var base64String = match.Groups[2].Value;

                var imageUrl = await _fileStorageManager.SaveFileAsync(base64String, "News", "Articles", Guid.NewGuid().ToString(), extension);
                newsArticle.Content = newsArticle.Content.Replace(matchString, imageUrl);
            }

            await _newsArticleRepository.AddAsync(newsArticle);
            await _newsArticleRepository.SaveChangesAsync();
        }

        public async Task EditNewsArticleAsync(NewsArticleDto newsArticleDto, string oldContent)
        {
            var newsArticle = await _newsArticleRepository.FindAsync(newsArticleDto.Id);

            if (newsArticle == null || newsArticle.ProfileId != _profileService.GetProfileId())
                throw new UnauthorizedAccessException("You are not authorized to edit this article.");

            string imgSrcPattern = @"<img.*?src=""((\/[^\s]+|\/[^\s]+|[a-zA-Z0-9_-]+(?:\/[^\s]*)?)|data:image\/(png|jpg|jpeg|gif);base64,([^\s]+))""/gm";

            var oldContentUrlMatches = Regex.Matches(oldContent, imgSrcPattern)
                .Cast<Match>()
                .Select(match => match.Groups[2].Value);

            var (newContentUrlMatches, newContentBase64Matches) = Regex.Matches(newsArticleDto.Content, imgSrcPattern)
                .Cast<Match>()
                .Aggregate(
                (base64: new List<string>(), urls: new List<string>()),
                (acc, match) =>
                {
                    // Group 1: base64 data URIs (if available)
                    var base64Match = match.Groups[1].Value;
                    if (!string.IsNullOrEmpty(base64Match))
                    {
                        acc.base64.Add(base64Match);
                    }

                    // Group 2: URLs or relative paths (if available)
                    var urlMatch = match.Groups[2].Value;
                    if (!string.IsNullOrEmpty(urlMatch))
                    {
                        acc.urls.Add(urlMatch);
                    }

                    return acc;
                }
            );

            //TO DO: Add new images to the file storage

            var removedUrls = oldContentUrlMatches.Except(newContentUrlMatches).ToList();

            foreach (var removedUrl in removedUrls)
            {
                await _fileStorageManager.DeleteFileAsync(removedUrl);
            }

            _mapper.Map(newsArticleDto, newsArticle);

            _newsArticleRepository.Update(newsArticle);
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
