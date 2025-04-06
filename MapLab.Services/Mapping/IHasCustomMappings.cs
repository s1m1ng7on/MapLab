using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace MapLab.Services.Mapping
{
    public interface IHasCustomMappings
    {
        void CreateMappings(IProfileExpression configuration, IServiceProvider services);
    }
}
