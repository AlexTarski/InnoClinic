using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Authorization.Business.Services;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;

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
        private const string _invalidPageAccessMessage = "Invalid page access";
        private const string _invalidClientMessage = "Invalid Client";
        private const string _validClientId = "inno-client";
        private const string _accountId = "0bca06f5-ddd2-4333-8b0a-d241fb994fc4";
        private const string _missingAccountId = "00000000-0000-0000-0000-000000000000";
        private const string _existingPhotoId = "d65781ba-5fb6-4b36-ba68-f3ca44763b9e";
        private const string _missingPhotoId = "00000000-0000-0000-0000-000000000000";
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
            var result = await IsEmailExisitsAsync(_existingUserEmail, new Account { Email = _existingUserEmail });

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsEmailExistsAsync_WhenUserNotFound_ReturnsFalse()
        {
            var result = await IsEmailExisitsAsync(_missingUserEmail, (Account?)null);

            Assert.That(result, Is.False);
        }

        #endregion

        #region IsDoctorProfileActiveAsync

        [Test]
        public async Task IsDoctorProfileActiveAsync_WhenHelperReturnsSuccess_ReturnsTrue()
        {
            var isActive = await IsDoctorProfileActiveAsync(true);

            Assert.That(isActive, Is.True);
        }

        [Test]
        public async Task IsDoctorProfileActiveAsync_WhenHelperReturnsFailure_ReturnsFalse()
        {
            var isActive = await IsDoctorProfileActiveAsync(false);

            Assert.That(isActive, Is.False);
        }

        #endregion

        #region GetProfileTypeAsync

        [Test]
        public async Task GetProfileTypeAsync_WhenSuccessAndValidEnum_ReturnsParsed()
        {
            var accountId = Guid.NewGuid();
            SetupProfilesApiHelperMock(accountId);

            var result = await _service.GetProfileTypeAsync(accountId);

            Assert.That(result, Is.EqualTo(ProfileType.Doctor));
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

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage!.Header, Does.Contain(_invalidPageAccessMessage));
            }
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientNull_ReturnsError()
        {
            SetupInteractionServiceMock(null);
            var result = await _service.GetClientIdAsync(_returnUrl);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage!.Header, Does.Contain(_invalidClientMessage));
            }
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientIdNull_ReturnsError()
        {
            SetupInteractionServiceMock(new Client { ClientId = null });
            var result = await _service.GetClientIdAsync(_returnUrl);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ErrorMessage, Is.Not.Null);
                Assert.That(result.ErrorMessage!.Header, Does.Contain(_invalidClientMessage));
            }
        }

        [Test]
        public async Task GetClientIdAsync_WhenClientIdProvided_ReturnsSuccess()
        {
            SetupInteractionServiceMock(new Client { ClientId = _validClientId });
            var result = await _service.GetClientIdAsync(_returnUrl);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.ClientId, Is.EqualTo(_validClientId));
            }
        }

        #endregion

        #region GetPhotoIdAsync

        private async Task<Guid> GetPhotoIdAsync(string accountId, string? expectedPhotoId)
        {
            var accountIdGuid = new Guid(accountId);
            Account? expectedResult;

            if(accountId == _missingAccountId)
            {
                expectedResult = (Account?)null;
            }
            else
            {
                expectedResult = new Account { Id = accountIdGuid, PhotoId = new Guid(expectedPhotoId!) };
            }

            _userManagerMock
                   .Setup(x => x.FindByIdAsync(accountId))
                   .ReturnsAsync(expectedResult);

            return await _service.GetPhotoIdAsync(accountIdGuid);
        }

        [TestCase(_accountId, _existingPhotoId, TestName = "GetPhotoIdAsync_WhenAccountExistsAndPhotoExists_ReturnsPhotoId")]
        [TestCase(_accountId, _missingPhotoId, TestName = "GetPhotoIdAsync_WhenAcountExistsAndPhotoIsMissing_ReturnsEmptyGuid")]
        public async Task GetPhotoIdAsync_WhenAccountExists_ReturnsPhotoId(string accountId, string expectedPhotoId)
        {
            var result = await GetPhotoIdAsync(accountId, expectedPhotoId);

            Assert.That(result, Is.EqualTo(new Guid(expectedPhotoId)));
        }

        [Test]
        public void GetPhotoIdAsync_WhenAccountNotFound_ThrowsKeyNotFoundException()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await GetPhotoIdAsync(_missingAccountId, null));
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

        private void SetupProfilesApiHelperMock(Guid accountId)
        {
            _profilesApiHelperMock
                .Setup(x => x.GetProfileTypeAsync(accountId))
                .ReturnsAsync(ProfileType.Doctor);
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

        private async Task<bool> IsEmailExisitsAsync(string email, Account? returnResult)
        {
            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(returnResult);

            return await _service.IsEmailExistsAsync(_existingUserEmail);
        }

        private async Task<bool> IsDoctorProfileActiveAsync(bool response)
        {
            var accountId = Guid.NewGuid();
            _profilesApiHelperMock
                .Setup(x => x.DoctorIsActiveAsync(accountId))
                .ReturnsAsync(response);

            return await _service.IsDoctorProfileActiveAsync(accountId);
        }
    }
}