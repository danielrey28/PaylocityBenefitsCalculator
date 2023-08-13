using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Api.Helpers
{
    public static class EnumExtensions
    {
        // This extension method allows for friendly display names of relationship types
        public static string? GetDisplayName(this Enum enumType)
        {
            var displayNameAttribute = enumType.GetType()
                .GetMember(enumType.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return displayNameAttribute == null ? enumType.ToString() : displayNameAttribute.Name;
        }
    }
}
