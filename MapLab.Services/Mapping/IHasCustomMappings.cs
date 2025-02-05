using AutoMapper;

namespace MapLab.Services.Mapping
{
    public interface IHasCustomMappings
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
