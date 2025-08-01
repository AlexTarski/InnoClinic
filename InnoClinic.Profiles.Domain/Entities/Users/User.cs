using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoClinic.Profiles.Domain.Entities.Users
{
    public abstract class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Guid AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
    }
}