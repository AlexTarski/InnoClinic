namespace InnoClinic.Shared.DataSeeding.Entities.Services
{
    public class Service : Entity
    {
        public Guid CategoryId { get; set; }
        public Guid SpecializationId { get; set; }
        public float Price { get; set; }
        public bool IsActive { get; set; }
    }
}