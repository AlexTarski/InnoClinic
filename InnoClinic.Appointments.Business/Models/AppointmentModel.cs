using System;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Appointments.Business.Models
{
    public record AppointmentModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required Guid PatientId { get; set; }
        [Required]
        public required Guid DoctorId { get; set; }
        [Required]
        public required Guid ServiceId { get; set; }
        [Required]
        public required DateTime Date { get; set; }
        [Required]
        public required DateTime Time { get; set; }
        [Required]
        public required bool IsApproved { get; set; }
    }
}