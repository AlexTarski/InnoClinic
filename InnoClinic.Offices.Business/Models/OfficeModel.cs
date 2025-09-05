using System.ComponentModel.DataAnnotations;

using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.Models
{
    public class OfficeModel
    {
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