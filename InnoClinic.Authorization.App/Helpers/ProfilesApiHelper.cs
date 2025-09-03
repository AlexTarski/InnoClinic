using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Helpers
{
    public class ProfilesApiHelper : IProfilesApiHelper
    {
        private readonly string _baseUrl = $"{AppUrls.ProfilesUrl}/api";
        private readonly string _doctorsEndpoint = "Doctors";
        private readonly string _profilesEndpoint = "Profiles";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProfilesApiHelper> _logger;

        public ProfilesApiHelper(IHttpClientFactory httpClientFactory, ILogger<ProfilesApiHelper> logger)
        {
            _httpClientFactory = httpClientFactory ??
                throw new DiNullReferenceException(nameof(httpClientFactory));
            _logger = logger ??
                throw new DiNullReferenceException(nameof(logger));
        }

        public async Task<HttpResponseMessage> GetDoctorProfileStatusAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetDoctorProfileStatusAsync));
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient
                .GetAsync($"{_baseUrl}/{_doctorsEndpoint}/{accountId}/status");
            Logger.DebugExitingMethod(_logger, nameof(GetDoctorProfileStatusAsync));

            return result;
        }

        public async Task<HttpResponseMessage> GetProfileTypeAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetProfileTypeAsync));
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient
                .GetAsync($"{_baseUrl}/{_profilesEndpoint}/{accountId}/type");
            Logger.DebugExitingMethod(_logger, nameof(GetProfileTypeAsync));

            return result;
        }
    }
}