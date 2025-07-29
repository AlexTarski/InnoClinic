namespace InnoClinic.Profiles.App.Models;

public class ReceptionistModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public Guid AccountId { get; set; }
    public Guid OfficeId { get; set; }
}