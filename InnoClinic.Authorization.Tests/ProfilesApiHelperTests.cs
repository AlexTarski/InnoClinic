using System.Net;
using System.Reflection;

using InnoClinic.Authorization.Business.Helpers;

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
        private const string _profileTypeResponse = "ProfileTypeResponseBody";
        private const string _doctorStatusEndpoint = "/api/Doctors/*/status";
        private const string _profileTypeEndpoint = "/api/Profiles/*/type";
        private const string _profilesApiHelperBaseUrlFieldName = "_baseUrl";
        private WireMockServer _server;
        private ProfilesApiHelper _helper;

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

            var httpClientFactory = new TestHttpClientFactory(httpClient);
            var logger = new NullLogger<ProfilesApiHelper>();

            _helper = new ProfilesApiHelper(httpClientFactory, logger);

            var baseUrlField = typeof(ProfilesApiHelper)
                .GetField(_profilesApiHelperBaseUrlFieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            baseUrlField.SetValue(
                _helper,
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
        public async Task GetDoctorProfileStatusAsync_WhenAPIAvailable_ReturnsExpectedContentAndStatusCode()
        {
            var accountId = Guid.NewGuid();

            var response = await _helper.GetDoctorProfileStatusAsync(accountId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var body = await response.Content.ReadAsStringAsync();
            Assert.That(body, Is.EqualTo(_doctorStatusResponse));
        }

        [Test]
        public async Task GetProfileTypeAsync_WhenAPIAvailable_ReturnsExpectedContentAndStatusCode()
        {
            var accountId = Guid.NewGuid();

            var response = await _helper.GetProfileTypeAsync(accountId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var body = await response.Content.ReadAsStringAsync();
            Assert.That(body, Is.EqualTo(_doctorStatusResponse));
        }
    }
}