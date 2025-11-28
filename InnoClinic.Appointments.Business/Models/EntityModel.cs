using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Appointments.Business.Models
{
    public abstract class EntityModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Name { get; set; }
    }
}