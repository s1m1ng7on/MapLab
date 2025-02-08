using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MapLab.Common.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum value)
            => value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? value.ToString();
    }
}
