namespace InnoClinic.Shared.DataSeeding.Entities
{
    public class Office
    {
        public Address Address { get; set; }
        public Guid PhotoId { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}