namespace InnoClinic.Profiles.Domain.Entities
{
    public class Receptionist
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid OfficeId { get; set; }
    }
}
