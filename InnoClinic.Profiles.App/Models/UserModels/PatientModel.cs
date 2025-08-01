namespace InnoClinic.Profiles.Business.Models.UserModels;

public class PatientModel : UserModel
{
    public DateTime DateOfBirth { get; set; }
    public bool IsLinkedToAccount { get; set; } = false;
}