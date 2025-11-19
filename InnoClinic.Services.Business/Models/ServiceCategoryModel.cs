using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Services.Business.Models
{
    public class ServiceCategoryModel : EntityModel
    {
        [Required]
        public TimeSpan TimeSlotSize { get; set; }
        public List<Service>? Services { get; set; }
    }
}