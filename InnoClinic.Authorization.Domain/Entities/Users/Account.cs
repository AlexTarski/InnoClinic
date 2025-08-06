using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Domain.Entities.Users
{
    public class Account : IdentityUser<Guid>
    {
        public Guid Photo_id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
