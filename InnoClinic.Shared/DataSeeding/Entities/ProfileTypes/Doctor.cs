using System.ComponentModel;

namespace InnoClinic.Shared.DataSeeding.Entities.ProfileTypes
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
}