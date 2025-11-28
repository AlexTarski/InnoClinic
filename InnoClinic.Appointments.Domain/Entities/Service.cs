using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Appointments.Domain.Entities
{
    public class Service : Entity
    {
        [Required]
        public Guid CategoryId { get; set; }
        public ServiceCategory? Category { get; set; }
        [Required]
        public Guid SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}