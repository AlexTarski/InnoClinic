using Microsoft.AspNetCore.Identity;

using IdentityServer4.Services;

using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Authorization.Business.Configuration;

namespace InnoClinic.Authorization.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIdentityServerInteractionService _interactionService;


        public AccountService(UserManager<Account> userManager,
            IHttpClientFactory httpClientFactory,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager), $"{nameof(userManager)} must not be null");
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory), $"{nameof(httpClientFactory)} must not be null");
            _interactionService = interactionService ??
                throw new ArgumentNullException(nameof(interactionService), $"{nameof(interactionService)} must not be null");
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> IsDoctorProfileActiveAsync(Guid accountId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient
                .GetAsync($"{AppUrls.ProfilesUrl}/api/Doctors/{accountId}/status");

            return result.IsSuccessStatusCode;
        }

        public async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient
                .GetAsync($"{AppUrls.ProfilesUrl}/api/Patients/accounts/{accountId}");

            if (result.IsSuccessStatusCode)
            {
                return ProfileType.Patient;
            }

            result = await httpClient
                .GetAsync($"{AppUrls.ProfilesUrl}/api/Doctors/accounts/{accountId}");

            if (result.IsSuccessStatusCode)
            {
                return ProfileType.Doctor;
            }

            result = await httpClient
                .GetAsync($"{AppUrls.ProfilesUrl}/api/Receptionists/accounts/{accountId}");

            if (result.IsSuccessStatusCode)
            {
                return ProfileType.Receptionist;
            }

            return ProfileType.UnknownProfile;
        }

        public async Task<IClientIdResult> GetClientIdAsync(string returnUrl)
        {
            var result = new ClientIdResult();
            var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);

            if (context == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = new MessageViewModel
                {
                    Title = "Error",
                    Header = "Invalid page access",
                    Message = "The authorization context could not be retrieved. Please, contact the administrator for more information."
                };

                return result;
            }

            var clientId = context.Client?.ClientId;

            if (clientId == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = new MessageViewModel
                {
                    Title = "Error",
                    Header = "Invalid Client",
                    Message = "The Client ID is not valid or not provided."
                };

                return result;
            }

            result.IsSuccess = true;
            result.ClientId = clientId;

            return result;
        }
        //only for Accounts, created through Register form on ClientUI site
        public async Task<IdentityResult> UpdateSelfCreatedUserAsync(Account user)
        {
            user.CreatedBy = user.Id;
            user.UpdatedBy = user.Id;
            return await _userManager.UpdateAsync(user);
        }
    }
}