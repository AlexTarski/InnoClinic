using System.Net;
using System.Reflection;

using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Shared.Exceptions;

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
    public class ProfilesApiHelperTests
    {
        private WireMockServer _server;
        private ProfilesApiHelper _helper;

        [SetUp]
        public void SetUp()
        {
            _server = WireMockServer.Start();

            _server
                .Given(Request.Create()
                    .WithPath("/api/Doctors/*/status")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("DoctorStatus"));

            _server
                .Given(Request.Create()
                    .WithPath("/api/Profiles/*/type")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("ProfileType"));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_server.Urls[0])
            };

            var httpClientFactory = new TestHttpClientFactory(httpClient);
            var logger = new NullLogger<ProfilesApiHelper>();

            _helper = new ProfilesApiHelper(httpClientFactory, logger);

            var baseUrlField = typeof(ProfilesApiHelper)
                .GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic);

            baseUrlField.SetValue(
                _helper,
                $"{_server.Urls[0]}/api"
            );
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
            _server.Dispose();
        }

        [Test]
        public async Task GetDoctorProfileStatusAsync_ReturnsExpectedContentAndStatusCode()
        {
            var accountId = Guid.NewGuid();

            var response = await _helper.GetDoctorProfileStatusAsync(accountId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var body = await response.Content.ReadAsStringAsync();
            Assert.That(body, Is.EqualTo("DoctorStatus"));
        }

        [Test]
        public async Task GetProfileTypeAsync_ReturnsExpectedContentAndStatusCode()
        {
            var accountId = Guid.NewGuid();

            var response = await _helper.GetProfileTypeAsync(accountId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var body = await response.Content.ReadAsStringAsync();
            Assert.That(body, Is.EqualTo("ProfileType"));
        }

        [Test]
        public void Constructor_NullHttpClientFactory_ThrowsDiNullReferenceException()
        {
            var logger = new NullLogger<ProfilesApiHelper>();
            Assert.Throws<DiNullReferenceException>(
                () => new ProfilesApiHelper(null, logger)
            );
        }

        [Test]
        public void Constructor_NullLogger_ThrowsDiNullReferenceException()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost") };
            var factory = new TestHttpClientFactory(httpClient);
            Assert.Throws<DiNullReferenceException>(
                () => new ProfilesApiHelper(factory, null)
            );
        }
    }
}