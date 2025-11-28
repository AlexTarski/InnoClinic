using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Appointments.Business.Models
{
    public class ServiceModel : EntityModel
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid SpecializationId { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}