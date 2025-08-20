using AutoMapper;

using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.API.MappingProfiles;

public class ProfilesMappingProfile : Profile
{
    public ProfilesMappingProfile()
    {
        CreateMap<RegisterViewModel, Account>()
            .ForMember(acc => acc.UserName, ex => ex.MapFrom(register => register.Email));
    }
}