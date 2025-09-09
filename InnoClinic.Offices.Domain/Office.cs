using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace InnoClinic.Offices.Domain
{
    [Collection("offices")]
    public class Office
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public Address Address { get; set; }

        public Guid PhotoId { get; set; }

        [Required(ErrorMessage = "RegistryPhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        public string RegistryPhoneNumber { get; set; }

        [Required(ErrorMessage = "isActive is required")]
        public bool IsActive { get; set; }
    }
}