using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace InnoClinic.Authorization.Domain.Entities.Users
{
    public abstract class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public Guid Ref2Id { get; set; }
        //[JsonIgnore]
        //public Account Account { get; set; }
    }
}