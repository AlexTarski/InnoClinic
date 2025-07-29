using AutoMapper;
using InnoClinic.Profiles.App.Models;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.API.MappingProfiles;

public class ProfilesMappingProfile : Profile
{
    public ProfilesMappingProfile()
    {
        CreateMap<Doctor, DoctorModel>().ReverseMap();
        CreateMap<Patient, PatientModel>().ReverseMap();
        CreateMap<Receptionist, ReceptionistModel>().ReverseMap();
    }
}