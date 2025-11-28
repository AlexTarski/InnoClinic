using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Appointments.Domain.Entities
{
    public class ServiceCategory : Entity
    {
        [Required]
        public TimeSpan TimeSlotSize { get; set; }
        public List<Service>? Services { get; set; }
    }
}