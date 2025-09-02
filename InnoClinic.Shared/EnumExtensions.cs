using System.Reflection;
using System.ComponentModel;

namespace InnoClinic.Shared
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the description associated with an enumeration value, as specified by the <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="value">The enumeration value for which to retrieve the description.</param>
        /// <returns>The description string defined by the <see cref="DescriptionAttribute"/> applied to the enumeration value.</returns>
        /// <exception cref="NullReferenceException">Thrown if the specified enumeration value does not have a <see cref="DescriptionAttribute"/>  or if the
        /// enumeration value is null.</exception>
        public static string GetStringValue(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                return attribute?.Description ?? throw new NullReferenceException($"{value} is null");
            }

            throw new NullReferenceException($"{value} is null");
        }
    }
}