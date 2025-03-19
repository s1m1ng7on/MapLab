using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace MapLab.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToStringByCulture(this DateTime dateTime, CultureInfo cultureInfo)
        {
            switch (cultureInfo.Name)
            {
                case "bg-BG":
                    return dateTime.ToString("dd MMM, yyyy", cultureInfo);
                case "en-US":
                    return dateTime.ToString("MMM dd, yyyy", cultureInfo);
                default:
                    return dateTime.ToString("MMM dd, yyyy", cultureInfo);
            }
        }
    }
}
