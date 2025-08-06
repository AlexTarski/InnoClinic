using AutoMapper;
using InnoClinic.Authorization.Business.Models.UserModels;
using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.API.MappingProfiles;

public class ProfilesMappingProfile : Profile
{
    public ProfilesMappingProfile()
    {
        //CreateMap<Account, AccountViewModel>().ReverseMap();
    }
}