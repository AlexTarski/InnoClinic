using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Helpers
{
    public class ProfilesApiHelper : IProfilesApiHelper
    {
        private readonly ILogger<ProfilesApiHelper> _logger;
        private readonly ProfilesApiClient _profilesApiClient;

        public ProfilesApiHelper(ILogger<ProfilesApiHelper> logger, ProfilesApiClient profilesApiClient)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _profilesApiClient = profilesApiClient ?? throw new DiNullReferenceException($"{nameof(profilesApiClient)}");
        }

        public async Task<bool> DoctorIsActiveAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(DoctorIsActiveAsync));
            var result = await _profilesApiClient.DoctorIsActiveAsync(accountId);
            Logger.DebugExitingMethod(_logger, nameof(DoctorIsActiveAsync));

            return result;
        }

        public async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(DoctorIsActiveAsync));
            var result = await _profilesApiClient.GetProfileTypeAsync(accountId);
            Logger.DebugExitingMethod(_logger, nameof(DoctorIsActiveAsync));

            return result;
        }
    }
}