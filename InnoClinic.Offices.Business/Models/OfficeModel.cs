using System.ComponentModel.DataAnnotations;

using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.Models
{
    public class OfficeModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please, enter the office’s address")]
        public required Address Address { get; set; }

        public Guid PhotoId { get; set; }

        [Required(ErrorMessage = "Please, enter the registry phone number")]
        [Phone(ErrorMessage = "You've entered an invalid phone number")]
        [RegularExpression(@"^\+\d{7,15}$", ErrorMessage = "You've entered an invalid phone number")]
        [DataType(DataType.PhoneNumber)]
        public required string RegistryPhoneNumber { get; set; }

        [Required(ErrorMessage = "Office status is required")]
        public bool IsActive { get; set; }
    }
}