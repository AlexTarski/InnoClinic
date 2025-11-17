using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Services.Business.Models
{
    public class SpecializationModel : EntityModel
    {
        [Required]
        public required bool IsActive { get; set; }
        public List<Service>? Services { get; set; }
    }
}