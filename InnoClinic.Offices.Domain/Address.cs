using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;

namespace InnoClinic.Offices.Domain
{
    public class Address
    {
        [BsonElement("city")]
        [Required]
        public string City { get; set; }

        [BsonElement("street")]
        [Required]
        public string Street { get; set; }

        [BsonElement("houseNumber")]
        [Required]
        public string HouseNumber { get; set; }

        [BsonElement("officeNumber")]
        [Required]
        public string OfficeNumber { get; set; }
    }
}