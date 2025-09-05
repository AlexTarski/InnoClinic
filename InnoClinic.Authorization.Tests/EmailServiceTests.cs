using InnoClinic.Authorization.Business.Services;
using InnoClinic.Authorization.Domain.Entities.Users;

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
        //TODO: Move strings to localization files
        private const string _testRecepientEmail = "test@localhost";
        private const string _testConfirmationLink = "http://confirm-link.com";
        private const string _emailBodyMessagePart = "Confirm Email";
        private const string _validToken = "valid-token";
        private const string _invalidToken = "invalid-token";
        private const string _missingUserId = "missing-user";
        private SimpleSmtpServer _smtpServer;
        private IConfiguration _config;
        private Mock<UserManager<Account>> _userManagerMock;
        private Mock<ILogger<EmailService>> _loggerMock;
        private EmailService _service;

        [SetUp]
        public void SetUp()
        {
            _smtpServer = SimpleSmtpServer.Start(2525);
            CreateConfiguration();
            CreateUserManagerMock();

            _loggerMock = new Mock<ILogger<EmailService>>();

            _service = new EmailService(
                _config,
                _userManagerMock.Object,
                _loggerMock.Object
            );
        }

        [TearDown]
        public void CleanUp()
        {
            _smtpServer.Stop();
            _smtpServer.Dispose();
        }

        [Test]
        public async Task SendVerificationMessageAsync_EmailIsSended()
        {
            var recipient = _testRecepientEmail;
            var link = _testConfirmationLink;

            await _service.SendVerificationMessageAsync(recipient, link);

            Assert.That(_smtpServer.ReceivedEmailCount, Is.EqualTo(1));

            var received = _smtpServer.ReceivedEmail[0];
            var body = received.MessageParts[0].BodyData;

            Assert.Multiple(() =>
            {
                Assert.That(body.Contains(_emailBodyMessagePart), Is.EqualTo(true));
                Assert.That(body.Contains(link), Is.EqualTo(true));
            });
        }

        [Test]
        public async Task ConfirmUserContactMethod_WhenConfirmationSucceeds_ReturnsTrue()
        {
            var userId = Guid.NewGuid();
            var token = _validToken;
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
        public void ConfirmUserContactMethod_WhenUserNotFound_Throws_KeyNotFoundException()
        {
            var userId = _missingUserId;
            var token = _invalidToken;

            _userManagerMock
                .Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync((Account?)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.ConfirmUserContactMethod(userId, token)
            );

            Assert.That(ex.Message, Does.Contain(userId));
        }

        private void CreateConfiguration()
        {
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
        }

        private void CreateUserManagerMock()
        {
            // UserManager requires(!) a IUserStore<Account> plus 8 optional params.
            var userStoreMock = new Mock<IUserStore<Account>>();
            _userManagerMock = new Mock<UserManager<Account>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );
        }
    }
}