using System.Reflection;

namespace InnoClinic.Profiles.Domain.Entities.Users
{
    public class Doctor : User
    {
        public DateTime DateOfBirth { get; set; }
        public Guid SpecializationId { get; set; }
        public Guid OfficeId { get; set; }
        public ushort CareerStartYear { get; set; }
        public DoctorStatus Status { get; set; }
    }
    
    public enum DoctorStatus
    {
        [StringValue("At work")] AtWork,
        [StringValue("On vacation")] OnVacation,
        [StringValue("Sick Day")] SickDay,
        [StringValue("Sick Leave")] SickLeave,
        [StringValue("Self-isolation")] SelfIsolation,
        [StringValue("Leave without pay")] LeaveWithoutPay,
        [StringValue("Inactive")] Inactive
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
