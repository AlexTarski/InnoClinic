using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Domain.Entities
{
    public class Receptionist : User
    {
        public Guid OfficeId { get; set; }
    }
}
