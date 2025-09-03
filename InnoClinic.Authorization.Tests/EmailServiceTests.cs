using InnoClinic.Authorization.Business.Services;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;

using netDumbster.smtp;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    public class EmailServiceTests
    {
        private const string _diErrorMessagePart = "Dependency Injection failed";
        private SimpleSmtpServer _smtpServer;
        private IConfiguration _config;
        private Mock<UserManager<Account>> _userManagerMock;
        private Mock<ILogger<EmailService>> _loggerMock;
        private EmailService _service;

        [SetUp]
        public void SetUp()
        {
            _smtpServer = SimpleSmtpServer.Start(2525);

            var inMemorySettings = new List<KeyValuePair<string, string?>>()
            {
                new("EmailSettings:From",         "noreply@innoclinic.com"),
                new("EmailSettings:DisplayName",  "InnoClinic"),
                new("EmailSettings:SmtpHost",     "localhost"),
                new("EmailSettings:SmtpPort",     "2525"),
                new("EmailSettings:CredUserName", null),
                new("EmailSettings:CredPassword", null),
                new("EmailSettings:EnableSsl",     "false")
            };

            _config = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemorySettings)
                                .Build();

            // UserManager requires(!) a IUserStore<Account> plus 8 optional params.
            var userStoreMock = new Mock<IUserStore<Account>>();
            _userManagerMock = new Mock<UserManager<Account>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );

            _loggerMock = new Mock<ILogger<EmailService>>();

            _service = new EmailService(
                _config,
                _userManagerMock.Object,
                _loggerMock.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _smtpServer.Stop();
            _smtpServer.Dispose();
        }

        #region Constructor DI Guards

        [Test]
        public void Constructor_NullConfiguration_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new EmailService(null!, _userManagerMock.Object, _loggerMock.Object)
            );

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        [Test]
        public void Constructor_NullUserManager_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new EmailService(_config, null!, _loggerMock.Object)
            );

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        [Test]
        public void Constructor_NullLogger_Throws()
        {
            var ex = Assert.Throws<DiNullReferenceException>(() =>
                new EmailService(_config, _userManagerMock.Object, null!)
            );

            Assert.That(ex.Message, Does.Contain(_diErrorMessagePart));
        }

        #endregion

        [Test]
        public async Task SendVerificationMessageAsync_EmailIsReceivedByMockServer()
        {
            var recipient = "test@localhost";
            var link = "http://confirm-link.com";

            await _service.SendVerificationMessageAsync(recipient, link);

            Assert.That(_smtpServer.ReceivedEmailCount, Is.EqualTo(1));

            var received = _smtpServer.ReceivedEmail[0];
            var body = received.MessageParts[0].BodyData;

            Assert.Multiple(() =>
            {
                Assert.That(body.Contains("Confirm Email"), Is.EqualTo(true));
                Assert.That(body.Contains(link), Is.EqualTo(true));
            });
        }

        [Test]
        public async Task ConfirmUserContactMethod_ReturnsTrue_WhenConfirmationSucceeds()
        {
            var userId = Guid.NewGuid();
            var token = "valid-token";
            var user = new Account { Id = userId };

            _userManagerMock
                .Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(m => m.ConfirmEmailAsync(user, token))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.ConfirmUserContactMethod(userId.ToString(), token);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void ConfirmUserContactMethod_Throws_WhenUserNotFound()
        {
            var userId = "missing-user";
            var token = "any-token";

            _userManagerMock
                .Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync((Account?)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.ConfirmUserContactMethod(userId, token)
            );

            Assert.That(ex.Message, Does.Contain(userId));
        }
    }
}