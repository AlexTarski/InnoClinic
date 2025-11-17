using System;
using System.ComponentModel.DataAnnotations;

using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Services.Business.Models
{
    public class ServiceModel : EntityModel
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}