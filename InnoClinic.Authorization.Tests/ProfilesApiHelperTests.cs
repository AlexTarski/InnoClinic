using System.Reflection;

using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Shared;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace InnoClinic.Authorization.Tests
{
    public class TestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public TestHttpClientFactory(HttpClient client)
        {
            _client = client;
        }

        public HttpClient CreateClient(string name)
        {
            return _client;
        }
    }

    [TestFixture]
    [Category("Unit")]
    public class ProfilesApiHelperTests
    {
        //TODO: Move strings to localization files
        private const string _doctorStatusResponse = "DoctorStatusResponseBody";
        private const string _profileTypeResponse = "Doctor";
        private const string _doctorStatusEndpoint = "/api/Doctors/*/status";
        private const string _profileTypeEndpoint = "/api/Profiles/*/type";
        private const string _profilesApiClientBaseUrlFieldName = "_baseUrl";
        private IConfiguration _config;
        private WireMockServer _server;
        private ProfilesApiHelper _helper;
        private ProfilesApiClient _profilesApiClient;

        [SetUp]
        public void SetUp()
        {
            _server = WireMockServer.Start();

            _server
                .Given(Request.Create()
                    .WithPath(_doctorStatusEndpoint)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody(_doctorStatusResponse));

            _server
                .Given(Request.Create()
                    .WithPath(_profileTypeEndpoint)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody(_profileTypeResponse));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_server.Urls[0])
            };

            CreateConfiguration();

            var logger = new NullLogger<ProfilesApiHelper>();
            var profilesApiClientLogger = new NullLogger<ProfilesApiClient>();
            _profilesApiClient = new ProfilesApiClient(profilesApiClientLogger, httpClient, _config);

            _helper = new ProfilesApiHelper(logger, _profilesApiClient);

            var baseUrlField = typeof(ProfilesApiClient)
                .GetField(_profilesApiClientBaseUrlFieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            baseUrlField.SetValue(
                _profilesApiClient,
                $"{_server.Urls[0]}/api"
            );
        }

        [TearDown]
        public void CleanUp()
        {
            _server.Stop();
            _server.Dispose();
        }

        [Test]
        public async Task GetDoctorProfileStatusAsync_WhenAPIAvailable_ReturnsExpectedContent()
        {
            var accountId = Guid.NewGuid();

            var response = await _helper.DoctorIsActiveAsync(accountId);

            Assert.That(response, Is.EqualTo(true));
        }

        [Test]
        public async Task GetProfileTypeAsync_WhenAPIAvailable_ReturnsExpectedContent()
        {
            var accountId = Guid.NewGuid();

            var response = await _helper.GetProfileTypeAsync(accountId);

            Assert.That(response, Is.EqualTo(Enum.Parse<ProfileType>(_profileTypeResponse)));
        }

        private void CreateConfiguration()
        {
            var inMemorySettings = new List<KeyValuePair<string, string?>>()
            {
                new("ProfilesApiSettings:BaseUrl", "https://localhost:7036/api"),
                new( "ProfilesApiSettings:ProfilesEndpoint", "Profiles"),
                new("ProfilesApiSettings:DoctorsEndpoint", "Doctors")
            };

            _config = new ConfigurationBuilder()
                        .AddInMemoryCollection(inMemorySettings)
                        .Build();
        }
    }
}