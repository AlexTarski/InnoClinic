namespace InnoClinic.Profiles.App.Models;

public class PatientModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Guid AccountId { get; set; }
}