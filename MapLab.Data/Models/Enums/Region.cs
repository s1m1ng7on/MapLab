using System.ComponentModel.DataAnnotations;

namespace MapLab.Data.Models.Enums
{
    //Object to future additions
    public enum Region
    {
        World = 0,
        Europe = 100,
        Asia = 200,
        [Display(Name = "North America")] NorthAmerica = 300,
        [Display(Name = "South America")] SouthAmerica = 400,
        Africa = 500,
        Australia = 600,
        Antarctica = 700,
        Other = 800
    }
}
