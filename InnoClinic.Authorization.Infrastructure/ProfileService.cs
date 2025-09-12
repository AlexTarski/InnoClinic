using System.Security.Claims;

using Duende.IdentityModel;

using IdentityServer4.Models;
using IdentityServer4.Services;

using InnoClinic.Authorization.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;

namespace InnoClinic.Authorization.Infrastructure
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<Account> _userManager;

        public ProfileService(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(r => new Claim(JwtClaimTypes.Role, r));
            context.IssuedClaims.AddRange(roleClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}