using System.Net;

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Authorization.Business.Services;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Moq;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    [Category("Unit")]
    public class AccountServiceTests
    {
        //TODO: Move strings to localization files
        private const string _returnUrl = "https://app/callback";
        private const string _existingUserEmail = "user@example.com";
        private const string _missingUserEmail = "missing@example.com";
        private const string _invalidEnumValue = "RandomText";
        private const string _invalidPageAccessMessage = "Invalid page access";
        private const string _invalidClientMessage = "Invalid Client";
        private const string _validClientId = "inno-client";
        private Mock<ILogger<AccountService>> _loggerMock;
        private Mock<UserManager<Account>> _userManagerMock;
        private Mock<IIdentityServerInteractionService> _interactionServiceMock;
        private Mock<IProfilesApiHelper> _profilesApiHelperMock;
        private AccountService _service;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AccountService>>();
            _interactionServiceMock = new Mock<IIdentityServerInteractionService>();
            _profilesApiHelperMock = new Mock<IProfilesApiHelper>();
            CreateUserManagerMock();

            _service = new AccountService(
                _loggerMock.Object,
                _userManagerMock.Object,
                _interactionServiceMock.Object,
                _profilesApiHelperMock.Object
            );
        }

        #region IsEmailExistsAsync

        [Test]
        public async Task IsEmailExistsAsync_WhenUserFound_ReturnsTrue()
        {
            _userManagerMock
                .Setup(x => x.FindByEmailAsync(_existingUserEmail))
                .ReturnsAsync(new Account { Email = _existingUserEmail });

            var exists = await _service.IsEmailExistsAsync(_existingUserEmail);

            Assert.That(exists, Is.EqualTo(true));
        }

        [Test]
        public async Task IsEmailExistsAsync_WhenUserNotFound_ReturnsFalse()
        {
            _userManagerMock
                .Setup(x => x.FindByEmailAsync(_missingUserEmail))
                .ReturnsAsync((Account?)null);

            var exists = await _service.IsEmailExistsAsync(_missingUserEmail);

            Assert.That(exists, Is.EqualTo(false));
        }

        #endregion

        #region IsDoctorProfileActiveAsync

        [Test]
        public async Task IsDoctorProfileActiveAsync_WhenHelperReturnsSuccess_ReturnsTrue()
        {
            var accountId = Guid.NewGuid();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _profilesApiHelperMock
                .Setup(x => x.GetDoctorProfileStatusAsync(accountId))
                .ReturnsAsync(response);

            var isActive = await _service.IsDoctorProfileActiveAsync(accountId);

            Assert.That(isActive, Is.EqualTo(true));
        }

        [Test]
        public async Task IsDoctorProfileActiveAsync_WhenHelperReturnsFailure_ReturnsFalse()
        {
            var accountId = Guid.NewGuid();
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _profilesApiHelperMock
                .Setup(x => x.GetDoctorProfileStatusAsync(accountId))
                .ReturnsAsync(response);

            var isActive = await _service.IsDoctorProfileActiveAsync(accountId);

            Assert.That(isActive, Is.EqualTo(false));
        }

        #endregion

        #region GetProfileTypeAsync

        [Test]
        public async Task GetProfileTypeAsync_WhenSuccessAndValidEnum_ReturnsParsed()
        {
            var accountId = Guid.NewGuid();
            var content = new StringContent(ProfileType.Doctor.ToString());
            SetupProfilesApiHelperMock(accountId, new HttpResponseMessage(HttpStatusCode.OK) { Content = content });

            var result = await _service.GetProfileTypeAsync(accountId);

            Assert.That(result, Is.EqualTo(ProfileType.Doctor));
        }

        [Test]
        public void GetProfileTypeAsync_WhenSuccessAndInvalidEnum_Throws_ProfileTypeApiException()
        {
            var accountId = Guid.NewGuid();
            var content = new StringContent(_invalidEnumValue);
            SetupProfilesApiHelperMock(accountId, new HttpResponseMessage(HttpStatusCode.OK) { Content = content });

            Assert.ThrowsAsync<ProfileTypeApiException>(async () =>
                await _service.GetProfileTypeAsync(accountId));
        }

        [Test]
        public void GetProfileTypeAsync_WhenFailureStatusCode_Throws_ProfileTypeApiException()
        {
            var accountId = Guid.NewGuid();
            var content = new StringContent(ProfileType.Receptionist.ToString());
            SetupProfilesApiHelperMock(accountId, new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = content });

            Assert.ThrowsAsync<ProfileTypeApiException>(async () =>
                await _service.GetProfileTypeAsync(accountId));
        }

        #endregion

        #region GetClientIdAsync

        [Test]
        public async Task GetClientIdAsync_WhenContextNull_ReturnsError()
        {
            _interactionServiceMock
                .Setup(x => x.GetAuthorizationContextAsync(_returnUrl))
                .ReturnsAsync((AuthorizationRequest?)null);

            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage.Header, Does.Contain(_invalidPageAccessMessage));
            });
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientNull_ReturnsError()
        {
            SetupInteractionServiceMock(null);
            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage.Header, Does.Contain(_invalidClientMessage));
            });
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientIdNull_ReturnsError()
        {
            SetupInteractionServiceMock(new Client { ClientId = null });
            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage.Header, Does.Contain(_invalidClientMessage));
            });
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientIdProvided_ReturnsSuccess()
        {
            SetupInteractionServiceMock(new Client { ClientId = _validClientId });
            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(true));
                Assert.That(result.ClientId, Is.EqualTo(_validClientId));
            });
        }

        #endregion

        #region UpdateSelfCreatedUserAsync

        [Test]
        public async Task UpdateSelfCreatedUserAsync_SetsAuditFieldsAndCallsUserManager()
        {
            var id = Guid.NewGuid();
            var user = new Account
            {
                Id = id,
                CreatedBy = Guid.Empty,
                UpdatedBy = Guid.Empty
            };
            var expected = IdentityResult.Success;

            _userManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<Account>()))
                .ReturnsAsync(expected);

            var result = await _service.UpdateSelfCreatedUserAsync(user);

            Assert.That(result, Is.EqualTo(expected));

            _userManagerMock.Verify(um =>
                um.UpdateAsync(It.Is<Account>(u =>
                    u.CreatedBy == id &&
                    u.UpdatedBy == id
                )),
                Times.Once);
        }

        #endregion

        private void CreateUserManagerMock()
        {
            // UserManager requires(!) a IUserStore<Account> plus 8 optional params.
            var userStoreMock = new Mock<IUserStore<Account>>();
            _userManagerMock = new Mock<UserManager<Account>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );
        }

        private void SetupProfilesApiHelperMock(Guid accountId, HttpResponseMessage response)
        {
            _profilesApiHelperMock
                .Setup(x => x.GetProfileTypeAsync(accountId))
                .ReturnsAsync(response);
        }

        private void SetupInteractionServiceMock(Client? client)
        {
            var context = new AuthorizationRequest
            {
                Client = client
            };

            _interactionServiceMock
                .Setup(x => x.GetAuthorizationContextAsync(_returnUrl))
                .ReturnsAsync(context);
        }
    }
}