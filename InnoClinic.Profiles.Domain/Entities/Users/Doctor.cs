using System.ComponentModel;
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
        [Description("At work")] AtWork,
        [Description("On vacation")] OnVacation,
        [Description("Sick Day")] SickDay,
        [Description("Sick Leave")] SickLeave,
        [Description("Self-isolation")] SelfIsolation,
        [Description("Leave without pay")] LeaveWithoutPay,
        [Description("Inactive")] Inactive
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