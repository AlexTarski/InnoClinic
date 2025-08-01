namespace InnoClinic.Profiles.Domain.Entities.Users
{
    public class Receptionist : User
    {
        public Guid OfficeId { get; set; }
    }
}