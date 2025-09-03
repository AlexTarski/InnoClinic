
namespace InnoClinic.Authorization.Business.Helpers
{
    public interface IProfilesApiHelper
    {
        Task<HttpResponseMessage> GetDoctorProfileStatusAsync(Guid accountId);
        Task<HttpResponseMessage> GetProfileTypeAsync(Guid accountId);
    }
}