using System.Net;

using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Authorization.Business.Helpers.ResultModels;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    [Category("Unit")]
    public class ProfilesApiHelperTests
    {
        private ProfilesApiHelper _helper;
        private Mock<ProfilesApiClient> _profilesApiClient;
        private IConfiguration _config;

        [SetUp]
        public void SetUp()
        {
            CreateConfiguration();
            var helperLogger = new NullLogger<ProfilesApiHelper>();
            var clientLogger = new NullLogger<ProfilesApiClient>();
            _profilesApiClient = new Mock<ProfilesApiClient>(MockBehavior.Strict, clientLogger, new HttpClient(), _config);
            _helper = new ProfilesApiHelper(helperLogger, _profilesApiClient.Object);
        }

        [Test]
        public async Task DoctorIsActiveAsync_WhenClientReturnsSuccess_ReturnsTrue()
        {
            var accountId = Guid.NewGuid();
            _profilesApiClient.Setup(c => c.DoctorIsActiveAsync(accountId))
                       .ReturnsAsync(new ProfilesApiResult<bool>(true, true, HttpStatusCode.OK, null));

            var result = await _helper.DoctorIsActiveAsync(accountId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DoctorIsActiveAsync_WhenClientReturnsFailure_ReturnsFalse()
        {
            var accountId = Guid.NewGuid();
            _profilesApiClient.Setup(c => c.DoctorIsActiveAsync(accountId))
                       .ReturnsAsync(new ProfilesApiResult<bool>(false, false, HttpStatusCode.NotFound, "Not Found"));

            var result = await _helper.DoctorIsActiveAsync(accountId);

            Assert.That(result, Is.False);
        }

        [Test]
        public void DoctorIsActiveAsync_WhenClientThrowsException_PropagatesException()
        {
            var accountId = Guid.NewGuid();
            _profilesApiClient.Setup(c => c.DoctorIsActiveAsync(accountId))
                       .ThrowsAsync(new InvalidOperationException("Network error"));

            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _helper.DoctorIsActiveAsync(accountId));
        }


        [Test]
        public async Task GetProfileTypeAsync_WhenClientReturnsSuccess_ReturnsProfileType()
        {
            var accountId = Guid.NewGuid();
            _profilesApiClient.Setup(c => c.GetProfileTypeAsync(accountId))
                       .ReturnsAsync(new ProfilesApiResult<ProfileType?>(true, ProfileType.Doctor, HttpStatusCode.OK, null));

            var result = await _helper.GetProfileTypeAsync(accountId);

            Assert.That(result, Is.EqualTo(ProfileType.Doctor));
        }

        [Test]
        public void GetProfileTypeAsync_WhenClientReturnsFailure_ThrowsProfileTypeApiException()
        {
            var accountId = Guid.NewGuid();
            _profilesApiClient.Setup(c => c.GetProfileTypeAsync(accountId))
                       .ReturnsAsync(new ProfilesApiResult<ProfileType?>(false, null, HttpStatusCode.InternalServerError, "Server error"));

            Assert.ThrowsAsync<ProfileTypeApiException>(
                async () => await _helper.GetProfileTypeAsync(accountId));
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