using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Business.Models;

public class DoctorModel : UserModel
{
    public DateTime DateOfBirth { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public ushort CareerStartYear { get; set; }
    public DoctorStatus Status { get; set; }
}