using System.Reflection;

namespace InnoClinic.Authorization.Business
{
    public enum ClientType
    {
        [StringValue("employee_ui")] EmployeeUI,
        [StringValue("client_ui")] ClientUI,
        [StringValue("profiles")] ProfilesAPI
    }

    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; }
        public StringValueAttribute(string value) => StringValue = value;
    }

    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attribute = field.GetCustomAttribute<StringValueAttribute>();
                return attribute?.StringValue ?? throw new NullReferenceException($"{value} is null");
            }

            throw new NullReferenceException($"{value} is null");
        }
    }
}