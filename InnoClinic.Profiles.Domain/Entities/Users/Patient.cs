using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Domain.Entities
{
    public class Patient : User
    {
        public DateTime DateOfBirth { get; set; }
        public bool IsLinkedToAccount { get; set; }
    }
}
