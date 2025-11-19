using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Services.Domain.Entities
{
    public class Specialization : Entity
    {
        [Required]
        public required bool IsActive { get; set; }
        public List<Service>? Services { get; set; }
    }
}