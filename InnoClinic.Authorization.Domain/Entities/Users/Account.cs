using Microsoft.AspNetCore.Identity;

namespace InnoClinic.Authorization.Domain.Entities.Users
{
    public class Account : IdentityUser<Guid>
    {
        public Guid Photo_id { get; set; } = Guid.Empty;
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UpdatedBy { get; set; } = Guid.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}