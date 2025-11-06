using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Authorization.Business.Helpers.ResultModels;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Helpers
{
    public class ProfilesApiClient
    {
        private readonly string sectionName = "ProfilesApiSettings";
        private readonly string _baseUrl;
        private readonly string _doctorsEndpoint;
        private readonly string _profilesEndpoint;
        private readonly ILogger<ProfilesApiClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ProfilesApiClient(ILogger<ProfilesApiClient> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _httpClient = httpClient ?? throw new DiNullReferenceException(nameof(httpClient));
            _configuration = configuration ?? throw new DiNullReferenceException(nameof(_configuration));

            var profilesApiConfig = configuration
                .GetSection(sectionName)
                .Get<ProfilesApiClientSettings>()
                          ?? throw new InvalidOperationException(
                                 $"Configuration section '{sectionName}' is missing or invalid.");

            _baseUrl = profilesApiConfig.BaseUrl;
            _doctorsEndpoint = profilesApiConfig.DoctorsEndpoint;
            _profilesEndpoint = profilesApiConfig.ProfilesEndpoint;
        }

        public async Task<DoctorStatusResult> DoctorIsActiveAsync(Guid accountId)
        {
            string endpointPath = $"{_doctorsEndpoint}/{accountId}/status";
            var result = await GetAsync(endpointPath, nameof(DoctorIsActiveAsync));

            if (result.IsSuccessStatusCode)
            {
                Logger.InfoSuccess(_logger, nameof(DoctorIsActiveAsync));
            }
            else
            {
                Logger.Warning(_logger, $"{result.Content}");
            }

            return result.IsSuccessStatusCode;
        }

        public async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            string endpointPath = $"{_profilesEndpoint}/{accountId}/type";
            var result = await GetAsync(endpointPath, nameof(GetProfileTypeAsync));

            if (result.IsSuccessStatusCode &&
                Enum.TryParse<ProfileType>(await result.Content.ReadAsStringAsync(), out ProfileType profileType))
            {
                LogMethodExit(Logger.InfoSuccess, nameof(GetProfileTypeAsync));

                return profileType;
            }

            LogMethodExit(Logger.WarningFailedDoAction, nameof(GetProfileTypeAsync));

            throw new ProfileTypeApiException();
        }

        private async Task<HttpResponseMessage> GetAsync(string endpoint, string callingMethodName)
        {
            Logger.DebugStartProcessingMethod(_logger, callingMethodName);

            var result = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");

            return result;
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
        private void LogMethodExit(Action<ILogger<ProfilesApiClient>, string> logMethod, string methodName)
        {
            logMethod(_logger, methodName);
            Logger.DebugExitingMethod(_logger, methodName);
        }
    }
}