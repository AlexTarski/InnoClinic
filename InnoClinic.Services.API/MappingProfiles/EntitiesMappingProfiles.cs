using AutoMapper;

using InnoClinic.Services.Business.Models;
using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Services.API.MappingProfiles
{
    public class EntitiesMappingProfiles : Profile
    {
        public EntitiesMappingProfiles()
        {
            CreateMap<Service, ServiceModel>().ReverseMap();
            CreateMap<ServiceCategory, ServiceCategoryModel>().ReverseMap();
            CreateMap<Specialization, SpecializationModel>().ReverseMap();
        }
    }
}