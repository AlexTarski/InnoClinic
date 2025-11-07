using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Authorization.Business.Helpers.ResultModels;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Helpers
{
    /// <summary>
    /// Provides a strongly-typed client for interacting with the Profiles API.
    ///  Returns structured results via <see cref="ProfilesApiResult{T}"/>.
    /// </summary>
    public class ProfilesApiClient
    {
        private const string sectionName = "ProfilesApiSettings";
        private readonly string _baseUrl;
        private readonly string _doctorsEndpoint;
        private readonly string _profilesEndpoint;
        private readonly ILogger<ProfilesApiClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilesApiClient"/> class.
        /// Reads configuration values from the <c>ProfilesApiSettings</c> section
        /// and sets up endpoint paths for operations.
        /// </summary>
        /// <param name="logger">Logger instance for diagnostic output.</param>
        /// <param name="httpClient">Injected <see cref="HttpClient"/> for making API calls.</param>
        /// <param name="configuration">Application configuration containing Profiles API settings.</param>
        /// <exception cref="DiNullReferenceException">Thrown if any dependency is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if configuration section is missing or invalid.</exception>
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

        /// <summary>
        /// Checks whether the doctor account associated with the given account ID is active.
        /// Calls the Profiles API doctor status endpoint and returns a structured result.
        /// </summary>
        /// <param name="accountId">Unique identifier of the doctor account.</param>
        /// <returns>
        /// A <see cref="ProfilesApiResult{T}"/> containing a boolean flag indicating success of request,
        /// doctor profile status (is active when status code 200), the HTTP status code, and the raw response content.
        /// </returns>
        public virtual async Task<ProfilesApiResult<bool>> DoctorIsActiveAsync(Guid accountId)
        {
            var response = await GetAsync($"{_baseUrl}/{_doctorsEndpoint}/{accountId}/status", nameof(DoctorIsActiveAsync));

            if (response.IsSuccessStatusCode)
            {
                Logger.InfoSuccess(_logger, nameof(DoctorIsActiveAsync));
            }
            else
            {
                Logger.Warning(_logger, $"{response.Content}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return new ProfilesApiResult<bool>(response.IsSuccessStatusCode, response.IsSuccessStatusCode, response.StatusCode, responseContent);
        }

        /// <summary>
        /// Retrieves the profile type for the account associated with the given account ID.
        /// Calls the Profiles API profile type endpoint and attempts to parse the response into a <see cref="ProfileType"/>.
        /// </summary>
        /// <param name="accountId">Unique identifier of the account.</param>
        /// <returns>
        /// A <see cref="ProfilesApiResult{T}"/> containing the parsed <see cref="ProfileType"/> if successful,
        /// or <c>null</c> if parsing failed, along with the HTTP status code and raw response content (<c>null</c> for successful response).
        /// </returns>
        public virtual async Task<ProfilesApiResult<ProfileType?>> GetProfileTypeAsync(Guid accountId)
        {
            var response = await GetAsync($"{_baseUrl}/{_profilesEndpoint}/{accountId}/type", nameof(GetProfileTypeAsync));
            bool isSuccess = response.IsSuccessStatusCode;

            if (isSuccess &&
                Enum.TryParse<ProfileType>(await response.Content.ReadAsStringAsync(), out ProfileType profileType))
            {
                LogMethodExit(Logger.InfoSuccess, nameof(GetProfileTypeAsync));

                return new ProfilesApiResult<ProfileType?>(isSuccess, profileType, response.StatusCode, null);
            }
            else
            {
                LogMethodExit(Logger.WarningFailedDoAction, nameof(GetProfileTypeAsync));
                var responseContent = await response.Content.ReadAsStringAsync();

                return new ProfilesApiResult<ProfileType?>(isSuccess, null, response.StatusCode, responseContent);
            }
        }

        private async Task<HttpResponseMessage> GetAsync(string endpointPath, string callingMethodName)
        {
            Logger.DebugStartProcessingMethod(_logger, callingMethodName);

            var response = await _httpClient.GetAsync($"{endpointPath}");

            return response;
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