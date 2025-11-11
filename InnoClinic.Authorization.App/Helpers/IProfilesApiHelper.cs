using InnoClinic.Shared;

namespace InnoClinic.Authorization.Business.Helpers
{
    public interface IProfilesApiHelper
    {
        Task<bool> DoctorIsActiveAsync(Guid accountId);
        Task<ProfileType> GetProfileTypeAsync(Guid accountId);
    }
}