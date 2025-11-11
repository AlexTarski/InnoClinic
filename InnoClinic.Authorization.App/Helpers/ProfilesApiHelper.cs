using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Helpers
{
    /// <summary>
    /// Provides higher-level helper methods for interacting with the Profiles API.
    /// Wraps <see cref="ProfilesApiClient"/> calls with logging and exception handling.
    /// </summary>
    public class ProfilesApiHelper : IProfilesApiHelper
    {
        private readonly ILogger<ProfilesApiHelper> _logger;
        private readonly ProfilesApiClient _profilesApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilesApiHelper"/> class.
        /// </summary>
        /// <param name="logger">Logger instance for diagnostic output.</param>
        /// <param name="profilesApiClient">Client used to call the Profiles API.</param>
        /// <exception cref="DiNullReferenceException">Thrown if any dependency is null.</exception>
        public ProfilesApiHelper(ILogger<ProfilesApiHelper> logger, ProfilesApiClient profilesApiClient)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _profilesApiClient = profilesApiClient ?? throw new DiNullReferenceException(nameof(profilesApiClient));
        }

        /// <summary>
        /// Determines whether the doctor account associated with the given account ID is active.
        /// </summary>
        /// <param name="accountId">Unique identifier of the doctor account.</param>
        /// <returns>
        /// <c>true</c> if the doctor account is active; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> DoctorIsActiveAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(DoctorIsActiveAsync));
            var result = await _profilesApiClient.DoctorIsActiveAsync(accountId);

            if (!result.IsSuccess)
            {
                Logger.Warning(_logger, $"{result.StatusCode} : {result.Message}");
            }

            Logger.DebugExitingMethod(_logger, nameof(DoctorIsActiveAsync));

            return result.IsSuccess;
        }

        /// <summary>
        /// Retrieves the profile type for the profile associated with the given account ID.
        /// Throws a <see cref="ProfileTypeApiException"/> if the API call fails.
        /// </summary>
        /// <param name="accountId">Unique identifier of the account.</param>
        /// <returns>
        /// The <see cref="ProfileType"/> value returned by the Profiles API.
        /// </returns>
        /// <exception cref="ProfileTypeApiException">
        /// Thrown if the API call fails or the profile type cannot be determined.
        /// </exception>
        public async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetProfileTypeAsync));
            var result = await _profilesApiClient.GetProfileTypeAsync(accountId);

            if (result.IsSuccess)
            {
                Logger.DebugExitingMethod(_logger, nameof(GetProfileTypeAsync));

                return (ProfileType)result.Result!;
            }

            Logger.Warning(_logger, $"{result.StatusCode} : {result.Message}");
            Logger.DebugExitingMethod(_logger, nameof(GetProfileTypeAsync));

            throw new ProfileTypeApiException($"Failed to get profile type for account {accountId}. {result.StatusCode} : {result.Message}");
        }
    }
}