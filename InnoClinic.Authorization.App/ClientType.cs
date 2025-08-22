using System.Reflection;
using System.ComponentModel;

namespace InnoClinic.Authorization.Business
{
    public enum ClientType
    {
        [Description("employee_ui")] EmployeeUI,
        [Description("client_ui")] ClientUI,
        [Description("profiles")] ProfilesAPI
    }
    
    public static class EnumExtensions
    {
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