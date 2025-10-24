using Microsoft.AspNetCore.Identity;

using InnoClinic.Shared;
using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.Business.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> IsEmailExistsAsync(string email);
        public Task<bool> IsDoctorProfileActiveAsync(Guid accountId);
        public Task<ProfileType> GetProfileTypeAsync(Guid accountId);
        public Task<IClientIdResult> GetClientIdAsync(string returnUrl);
        public Task<Guid> GetPhotoIdAsync(Guid accountId);
        public Task<IdentityResult> UpdateSelfCreatedUserAsync(Account user);
    }
}