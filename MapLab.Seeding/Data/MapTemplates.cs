using MapLab.Data.Entities;
using MapLab.Data.Models.Enums;
using MapLab.Services.Models;

namespace MapLab.Seeding.Data
{
    internal class MapTemplates
    {
        public static List<(MapTemplateDto, string)> InitialMapTemplates = new List<(MapTemplateDto, string)>()
        {
            (new MapTemplateDto() { Name = "Austria", Region = Region.Europe }, "austria.geojson"),
            (new MapTemplateDto() { Name = "Belgium", Region = Region.Europe }, "belgium.geojson"),
            (new MapTemplateDto() { Name = "Bulgaria", Region = Region.Europe }, "bulgaria.geojson"),
            (new MapTemplateDto() { Name = "Denmark", Region = Region.Europe }, "denmark.geojson"),
            (new MapTemplateDto() { Name = "France", Region = Region.Europe }, "france.geojson"),
            (new MapTemplateDto() { Name = "Germany", Region = Region.Europe }, "germany.geojson"),
            (new MapTemplateDto() { Name = "Greece", Region = Region.Europe }, "greece.geojson"),
            (new MapTemplateDto() { Name = "Hungary", Region = Region.Europe }, "hungary.geojson"),
            (new MapTemplateDto() { Name = "Ireland", Region = Region.Europe }, "ireland.geojson"),
            (new MapTemplateDto() { Name = "Italy", Region = Region.Europe }, "italy.geojson"),
            (new MapTemplateDto() { Name = "Poland", Region = Region.Europe }, "poland.geojson"),
            (new MapTemplateDto() { Name = "Portugal", Region = Region.Europe }, "portugal.geojson"),
            (new MapTemplateDto() { Name = "Romania", Region = Region.Europe }, "romania.geojson"),
            (new MapTemplateDto() { Name = "Serbia", Region = Region.Europe }, "serbia.geojson"),
            (new MapTemplateDto() { Name = "Spain", Region = Region.Europe }, "spain.geojson"),
            (new MapTemplateDto() { Name = "Sweden", Region = Region.Europe }, "sweden.geojson"),
            (new MapTemplateDto() { Name = "Switzerland", Region = Region.Europe }, "switzerland.geojson"),
            (new MapTemplateDto() { Name = "The Netherlands", Region = Region.Europe }, "the-netherlands.geojson"),
            (new MapTemplateDto() { Name = "Turkey", Region = Region.Europe }, "turkey.geojson"),
            (new MapTemplateDto() { Name = "United Kingdom", Region = Region.Europe }, "united-kingdom.geojson"),
        };
    }
}