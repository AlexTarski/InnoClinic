using AutoMapper;
using InnoClinic.Appointments.Business.Models;
using InnoClinic.Appointments.Domain.Entities;

namespace InnoClinic.Appointments.API.MappingProfiles
{
    public class AppointmentsMappingProfiles : Profile
    {
        public AppointmentsMappingProfiles()
        {
            CreateMap<Appointment, AppointmentModel>().ReverseMap();
        }
    }
}