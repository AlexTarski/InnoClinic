using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Domain.Entities
{
    public enum DoctorStatus
    {
        AtWork,
        OnVacation,
        SickDay,
        SickLeave,
        SelfIsolation,
        LeaveWithoutPay,
        Inactive
    }

    public class Doctor : User
    {
        public DateTime DateOfBirth { get; set; }
        public Guid SpecializationId { get; set; }
        public Guid OfficeId { get; set; }
        public ushort CareerStartYear { get; set; }
        public DoctorStatus Status { get; set; }
    }
}
