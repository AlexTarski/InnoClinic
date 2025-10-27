using System.Security.Claims;

using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

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

            var claims = roles.Select(r => new Claim(JwtClaimTypes.Role, r)).ToList();

            if (context.RequestedClaimTypes.Contains("photo_id"))
            {
                claims.Add(new Claim("photo_id", user.PhotoId.ToString()));
            }

            if (context.RequestedClaimTypes.Contains(JwtClaimTypes.Email))
            {
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email!));
            }

            if (context.RequestedClaimTypes.Contains(JwtClaimTypes.EmailVerified))
            {
                claims.Add(new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}