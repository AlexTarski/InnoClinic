using AutoMapper;
using InnoClinic.Profiles.Business.Models;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities;
using InnoClinic.Profiles.Domain.Entities.Users;

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