using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Offices.Domain
{
    [Collection("offices")]
    public class Office
    {
        [BsonId]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public Address Address { get; set; }

        public Guid PhotoId { get; set; }

        [Required(ErrorMessage = "RegistryPhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        public string RegistryPhoneNumber { get; set; }

        [Required(ErrorMessage = "isActive is required")]
        public bool isActive { get; set; }
    }
}