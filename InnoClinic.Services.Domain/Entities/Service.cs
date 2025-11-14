using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Services.Domain.Entities
{
    public class Service : Entity
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public required ServiceCategory Category { get; set; }
        [Required]
        public Guid SpecializationId { get; set; }
        [Required]
        public required Specialization Specialization { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}