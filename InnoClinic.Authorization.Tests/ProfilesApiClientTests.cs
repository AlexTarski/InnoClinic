using System.Net;

using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Authorization.Business.Helpers.ResultModels;
using InnoClinic.Shared;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    [Category("Unit")]
    public class ProfilesApiClientTests
    {
        private WireMockServer _server;
        private IConfiguration _config;
        private ILogger<ProfilesApiClient> _logger;

        private HttpClient _httpClient;
        private ProfilesApiClient _client;

        [SetUp]
        public void SetUp()
        {
            _server = WireMockServer.Start();

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ProfilesApiSettings:BaseUrl", $"{_server.Urls[0]}/api" },
                    { "ProfilesApiSettings:ProfilesEndpoint", "Profiles" },
                    { "ProfilesApiSettings:DoctorsEndpoint", "Doctors" }
                })
                .Build();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_server.Urls[0])
            };

            _logger = new NullLogger<ProfilesApiClient>();
            _client = new ProfilesApiClient(_logger, _httpClient, _config);
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
            _server.Dispose();
            _httpClient.Dispose();
        }

        #region DoctorIsActiveAsync
        [TestCase("")]
        [TestCase("true")]
        [TestCase("testValue")]
        public async Task DoctorIsActiveAsync_WhenApiReturns200WithBodyOrEmptyBody_ReturnsSuccessWithTrue(string responseBody)
        {
            var result = await GetDoctorProfileStatusAsync(HttpStatusCode.OK, responseBody);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Result, Is.True);
            }
        }

        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public async Task DoctorIsActiveAsync_WhenApiReturnsNonSuccessStatusCode_ReturnsFailureWithFalse(
            HttpStatusCode statusCode, string responseBody = "TestResponse")
        {
            var result = await GetDoctorProfileStatusAsync(statusCode, responseBody);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Result, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo(statusCode));
            }
        }
        #endregion

        #region GetProfileTypeAsync
        [Test]
        public async Task GetProfileTypeAsync_WhenApiReturns200Doctor_ReturnsSuccessWithDoctorEnum()
        {
            var result = await GetProfileTypeAsync(HttpStatusCode.OK, "Doctor");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Result, Is.EqualTo(ProfileType.Doctor));
            }
        }

        [TestCase("")]
        [TestCase("InvalidType")]
        public async Task GetProfileTypeAsync_WhenApiReturns200InvalidEnumOrEmptyBody_ReturnsSuccessWithNullResult(string responseBody)
        {
            var result = await GetProfileTypeAsync(HttpStatusCode.OK, responseBody);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Message, Is.EqualTo(responseBody));
            }
        }

        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public async Task GetProfileTypeAsync_WhenApiReturnsNonSuccessStatusCode_ReturnsFailureWithNullResultAndStatusCode(
            HttpStatusCode statusCode, string responseBody = "TestResponse")
        {
            var result = await GetProfileTypeAsync(statusCode, responseBody);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Result, Is.Null);
                Assert.That(result.StatusCode, Is.EqualTo(statusCode));
                Assert.That(result.Message, Is.EqualTo(responseBody));
            }
        }
        #endregion

        private async Task<ProfilesApiResult<bool>> GetDoctorProfileStatusAsync(HttpStatusCode statusCode, string responseBody)
        {
            var doctorId = Guid.NewGuid();

            SetupHttpClientWithDoctorIsActiveEndpoint(doctorId, statusCode, responseBody);

            return await _client.DoctorIsActiveAsync(doctorId);
        }

        private async Task<ProfilesApiResult<ProfileType?>> GetProfileTypeAsync(HttpStatusCode statusCode, string responseBody)
        {
            var profileId = Guid.NewGuid();

            SetupHttpClientWithProfileTypeEndpoint(profileId, statusCode, responseBody);

            return await _client.GetProfileTypeAsync(profileId)!;
        }

        private void SetupHttpClientWithDoctorIsActiveEndpoint(Guid doctorId, HttpStatusCode statusCode, string responseBody)
        {
            string endpoint = $"/api/Doctors/{doctorId}/status";
            SetupHttpClient(endpoint, statusCode, responseBody);
        }

        private void SetupHttpClientWithProfileTypeEndpoint(Guid profileId, HttpStatusCode statusCode, string responseBody)
        {
            string endpoint = $"/api/Profiles/{profileId}/type";
            SetupHttpClient(endpoint, statusCode, responseBody);
        }

        private void SetupHttpClient(string endpoint, HttpStatusCode statusCode, string responseBody)
        {
            _server.Given(Request.Create()
                    .WithPath(endpoint)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(statusCode)
                    .WithBody(responseBody));
        }
    }
}