using InnoClinic.Shared.DataSeeding.Entities.ProfileTypes;

namespace InnoClinic.Shared.DataSeeding.Entities
{
    public class Account<T>
        where T : User
    {
        public string Email;
        public string PasswordHash;
        public string PhoneNumber;
        public Guid PhotoId;
        public T Profile;
    }
}