using AutoMapper;
using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Web.Models.Home;

namespace MapLab.Web.Models.News
{
    public class NewsSectionViewModel : IHasCustomMappings
    {
        public List<NewsArticleCardViewModel>? Articles { get; set; }

        public void CreateMappings(IProfileExpression configuration, IServiceProvider services)
        {
            configuration.CreateMap<PaginationDto<NewsArticleDto>, NewsSectionViewModel>()
                .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.Items));

            configuration.CreateMap<PaginationDto<NewsArticleDto>, HomeIndexViewModel>()
                .IncludeBase<PaginationDto<NewsArticleDto>, NewsSectionViewModel>();
        }
    }
}
