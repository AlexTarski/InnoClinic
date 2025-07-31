namespace InnoClinic.Profiles.Domain.Entities
{
    public enum DoctorStatus
    {
        At_work,
        On_vacation,
        Sick_Day,
        Sick_Leave,
        Self_isolation,
        Leave_without_pay,
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
