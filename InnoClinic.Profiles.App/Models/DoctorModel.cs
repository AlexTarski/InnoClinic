namespace InnoClinic.Profiles.App.Models;

public class DoctorModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Guid AccountId { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public ushort CareerStartYear { get; set; }
    public string Status { get; set; }
}