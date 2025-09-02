using IdentityServer4.Services;

using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIdentityServerInteractionService _interactionService;

        public AccountService(
            ILogger<AccountService> logger,
            UserManager<Account> userManager,
            IHttpClientFactory httpClientFactory,
            IIdentityServerInteractionService interactionService)
        {
            _logger = logger ??
                throw new DiNullReferenceException(nameof(logger));
            _userManager = userManager ??
                throw new DiNullReferenceException(nameof(userManager));
            _httpClientFactory = httpClientFactory ??
                throw new DiNullReferenceException(nameof(httpClientFactory));
            _interactionService = interactionService ??
                throw new DiNullReferenceException(nameof(interactionService));
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            LogMethodStart(nameof(IsEmailExistsAsync), nameof(_userManager.FindByEmailAsync));

            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> IsDoctorProfileActiveAsync(Guid accountId)
        {
            LogMethodStart(nameof(IsDoctorProfileActiveAsync), nameof(HttpClient.GetAsync));
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient
                .GetAsync($"{AppUrls.ProfilesUrl}/api/Doctors/{accountId}/status");

            Logger.InfoBoolResult(_logger, nameof(IsDoctorProfileActiveAsync), result.IsSuccessStatusCode.ToString());
            Logger.DebugExitingMethod(_logger, nameof(IsDoctorProfileActiveAsync));

            return result.IsSuccessStatusCode;
        }

        public async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            LogMethodStart(nameof(GetProfileTypeAsync), nameof(HttpClient.GetAsync));
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient
                .GetAsync($"{AppUrls.ProfilesUrl}/api/Profiles/{accountId}/type");

            Logger.InfoTryDoAction(_logger, nameof(GetProfileTypeAsync));

            if (result.IsSuccessStatusCode &&
                Enum.TryParse<ProfileType>(await result.Content.ReadAsStringAsync(), out ProfileType profileType))
            {
                LogMethodExit(Logger.InfoSuccess, nameof(GetProfileTypeAsync));

                return profileType;
            }

            LogMethodExit(Logger.WarningFailedDoAction, nameof(GetProfileTypeAsync));

            return ProfileType.UnknownProfile;
        }

        public async Task<IClientIdResult> GetClientIdAsync(string returnUrl)
        {
            LogMethodStart(nameof(GetClientIdAsync), nameof(GetClientIdAsync));
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

                LogMethodExit(Logger.WarningFailedDoAction, nameof(GetClientIdAsync));

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

                LogMethodExit(Logger.WarningFailedDoAction, nameof(GetClientIdAsync));

                return result;
            }

            result.IsSuccess = true;
            result.ClientId = clientId;

            LogMethodExit(Logger.InfoSuccess, nameof(GetClientIdAsync));

            return result;
        }

        //TODO: review this method (differ from creating other accounts by receptionists)
        //only for Accounts, created through Register form in ClientUI site
        public async Task<IdentityResult> UpdateSelfCreatedUserAsync(Account user)
        {
            Logger.DebugStartProcessingMethod(_logger,
                nameof(UpdateSelfCreatedUserAsync));
            user.CreatedBy = user.Id;
            user.UpdatedBy = user.Id;
            Logger.DebugExitingMethod(_logger, nameof(UpdateSelfCreatedUserAsync));

            return await _userManager.UpdateAsync(user);
        }

        private void LogMethodStart(string methodName, string actionName)
        {
            Logger.DebugStartProcessingMethod(_logger, methodName);
            Logger.InfoTryDoAction(_logger, actionName);
        }

        /// <summary>
        /// Logs the exit of a method using the specified logging action and method name.
        /// Use <see cref="Logger.InfoSuccess"/> for successful method execution,
        /// and <see cref="Logger.WarningFailedDoAction"/> if execution failed.
        /// </summary>
        /// <param name="logMethod">
        /// The logging action to execute, which takes an <see cref="ILogger{TCategoryName}"/> instance
        /// and the name of the method being exited.
        /// </param>
        /// <param name="methodName">
        /// The name of the method that is exiting. This value is included in the log entry.
        /// </param>
        private void LogMethodExit(Action<ILogger<AccountService>, string> logMethod, string methodName)
        {
            logMethod(_logger, methodName);
            Logger.DebugExitingMethod(_logger, methodName);
        }
    }
}