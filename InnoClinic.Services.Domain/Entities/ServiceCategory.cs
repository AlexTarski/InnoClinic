using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Services.Domain.Entities
{
    public class ServiceCategory : Entity
    {
        [Required]
        public TimeSpan TimeSlotSize { get; set; }
        [Required]
        public required List<Service> Services { get; set; }
    }
}