namespace InnoClinic.Profiles.Domain.Entities.Users
{
    public class Patient : User
    {
        public DateTime DateOfBirth { get; set; }
        public bool IsLinkedToAccount { get; set; }
    }
}
