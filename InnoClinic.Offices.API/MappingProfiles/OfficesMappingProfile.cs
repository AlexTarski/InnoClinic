using AutoMapper;

using InnoClinic.Offices.Business.Models;
using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.API.MappingProfiles
{
    public class ProfilesMappingProfile : Profile
    {
        public ProfilesMappingProfile()
        {
            CreateMap<OfficeModel, Office>().ReverseMap();
        }
    }
}