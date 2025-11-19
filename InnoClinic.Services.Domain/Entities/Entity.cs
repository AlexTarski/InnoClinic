using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Services.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Name { get; set; }
    }
}