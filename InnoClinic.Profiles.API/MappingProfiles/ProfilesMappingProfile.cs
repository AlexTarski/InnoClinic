using AutoMapper;
using InnoClinic.Profiles.Business.Models;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.API.MappingProfiles;

public class ProfilesMappingProfile : Profile
{
    public ProfilesMappingProfile()
    {
        CreateMap<Doctor, DoctorModel>().ReverseMap();
        CreateMap<Patient, PatientModel>().ReverseMap();
        CreateMap<Receptionist, ReceptionistModel>().ReverseMap();
        CreateMap<Account, AccountModel>().ReverseMap();
    }
}