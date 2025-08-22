using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Business.Interfaces
{
    public interface IProfilesService
    {
        public Task<ProfileType> GetProfileTypeAsync(Guid accountId);
    }
}