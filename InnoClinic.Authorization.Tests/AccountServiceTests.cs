using System.Net;

using IdentityServer4.Models;
using IdentityServer4.Services;

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
    public class AccountServiceTests
    {
        private const string _returnUrl = "https://app/callback";
        private const string _diErrorMessagePart = "Dependency Injection failed";
        private Mock<ILogger<AccountService>> _loggerMock;
        private Mock<UserManager<Account>> _userManagerMock;
        private Mock<IIdentityServerInteractionService> _interactionServiceMock;
        private Mock<IProfilesApiHelper> _profilesApiHelperMock;
        private AccountService _service;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AccountService>>();

            // UserManager requires(!) a IUserStore<Account> plus 8 optional params.
            var userStore = new Mock<IUserStore<Account>>();
            _userManagerMock = new Mock<UserManager<Account>>(
                            userStore.Object,
                            null, null, null, null, null, null, null, null);
            _interactionServiceMock = new Mock<IIdentityServerInteractionService>();
            _profilesApiHelperMock = new Mock<IProfilesApiHelper>();

            _service = new AccountService(
                _loggerMock.Object,
                _userManagerMock.Object,
                _interactionServiceMock.Object,
                _profilesApiHelperMock.Object
            );
        }

        #region Constructor DI Guards

        [Test]
        public void Constructor_NullLogger_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new AccountService(
                    null!,
                    _userManagerMock.Object,
                    _interactionServiceMock.Object,
                    _profilesApiHelperMock.Object));

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        [Test]
        public void Constructor_NullUserManager_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new AccountService(
                    _loggerMock.Object,
                    null!,
                    _interactionServiceMock.Object,
                    _profilesApiHelperMock.Object));

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        [Test]
        public void Constructor_NullInteractionService_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new AccountService(
                    _loggerMock.Object,
                    _userManagerMock.Object,
                    null!,
                    _profilesApiHelperMock.Object));

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        [Test]
        public void Constructor_NullProfilesApiHelper_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new AccountService(
                    _loggerMock.Object,
                    _userManagerMock.Object,
                    _interactionServiceMock.Object,
                    null!));

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        #endregion

        #region IsEmailExistsAsync

        [Test]
        public async Task IsEmailExistsAsync_WhenUserFound_ReturnsTrue()
        {
            const string email = "user@example.com";
            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(new Account { Email = email });

            var exists = await _service.IsEmailExistsAsync(email);

            Assert.That(exists, Is.EqualTo(true));
        }

        [Test]
        public async Task IsEmailExistsAsync_WhenUserNotFound_ReturnsFalse()
        {
            const string email = "missing@example.com";
            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((Account?)null);

            var exists = await _service.IsEmailExistsAsync(email);

            Assert.That(exists, Is.EqualTo(false));
        }

        #endregion

        #region IsDoctorProfileActiveAsync

        [Test]
        public async Task IsDoctorProfileActiveAsync_WhenHelperReturnsSuccess_True()
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
        public async Task IsDoctorProfileActiveAsync_WhenHelperReturnsFailure_False()
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
        public void GetProfileTypeAsync_WhenSuccessAndInvalidEnum_Throws()
        {
            var accountId = Guid.NewGuid();
            var content = new StringContent("NonExistent");
            SetupProfilesApiHelperMock(accountId, new HttpResponseMessage(HttpStatusCode.OK) { Content = content });

            Assert.ThrowsAsync<ProfileTypeApiException>(async () =>
                await _service.GetProfileTypeAsync(accountId));
        }

        [Test]
        public void GetProfileTypeAsync_WhenFailureStatusCode_Throws()
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
                Assert.That(result.ErrorMessage.Header, Does.Contain("Invalid page access"));
            });
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientNull_ReturnsError()
        {
            var ctx = new AuthorizationRequest { Client = null };
            SetupInteractionServiceMock(ctx);

            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage.Header, Does.Contain("Invalid Client"));
            });
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientIdNull_ReturnsError()
        {
            var ctx = new AuthorizationRequest
            {
                Client = new Client { ClientId = null }
            };

            SetupInteractionServiceMock(ctx);

            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage.Header, Does.Contain("Invalid Client"));
            });
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientIdProvided_ReturnsSuccess()
        {
            const string expectedClientId = "inno-client";
            var ctx = new AuthorizationRequest
            {
                Client = new Client { ClientId = expectedClientId }
            };

            SetupInteractionServiceMock(ctx);

            var result = await _service.GetClientIdAsync(_returnUrl);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.EqualTo(true));
                Assert.That(result.ClientId, Is.EqualTo(expectedClientId));
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

        private void SetupProfilesApiHelperMock(Guid accountId, HttpResponseMessage response)
        {
            _profilesApiHelperMock
                .Setup(x => x.GetProfileTypeAsync(accountId))
                .ReturnsAsync(response);
        }

        private void SetupInteractionServiceMock(AuthorizationRequest? context)
        {
            _interactionServiceMock
                .Setup(x => x.GetAuthorizationContextAsync(_returnUrl))
                .ReturnsAsync(context);
        }
    }
}