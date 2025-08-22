using InnoClinic.Shared;

namespace InnoClinic.Profiles.Business.Interfaces
{
    public interface IProfilesService
    {
        public Task<ProfileType> GetProfileTypeAsync(Guid accountId);
    }
}