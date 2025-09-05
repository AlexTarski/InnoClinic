using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Authorization.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Account> _userManager;

        public EmailService(IConfiguration configuration,
            UserManager<Account> userManager,
            ILogger<EmailService> logger)
        {
            _logger = logger ??
                throw new DiNullReferenceException(nameof(logger));
            _configuration = configuration ??
                throw new DiNullReferenceException(nameof(configuration));
            _userManager = userManager ??
                throw new DiNullReferenceException(nameof(userManager));
        }

        public async Task SendVerificationMessageAsync(string userAddress, string confirmationLink)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(SendVerificationMessageAsync));
            var emailConfig = _configuration
                         .GetSection("EmailSettings")
                         .Get<EmailSettings>();
            MailAddress from = new(emailConfig.From, emailConfig.DisplayName);
            MailAddress to = new(userAddress);
            MailMessage m = new(from, to)
            {
                Subject = "Email verification link",
                Body = $@"
                <div style='font-family:Segoe UI, sans-serif; font-size:16px; color:#333;'>
                    <div style='text-align:center; margin-bottom:20px;'>
                        <img src='{AppUrls.AuthUrl}/assets/innoclinic-logo.png' alt='InnoClinic Logo' style='height:60px;' />
                    </div>
                    <p>Thank you for registering with <strong>InnoClinic</strong>.</p>
                    <p>Please confirm your InnoClinic account by clicking the link below:</p>
                    <p><a href='{HtmlEncoder.Default.Encode(confirmationLink)}' style='color:#3498db;'>Confirm Email</a></p>
                    <hr style='margin:20px 0; border:none; border-top:1px solid #ccc;' />
                    <p style='font-size:14px; color:#777;'>® 2025 InnoClinic. All rights reserved.</p>
                    <p style='font-size:14px;'>
                        <a href='{AppUrls.ClientUiUrl}' style='color:#777;'>InnoClinic</a> |
                        <a href='https://innowise.com/' style='color:#777;'>Innowise</a> |
                        <a href='https://innowise.com/careers/' style='color:#777;'>Careers</a> |
                        <a href='https://innowise.com/contact-us/' style='color:#777;'>Contact Us</a>
                    </p>
                </div>",
                IsBodyHtml = true
            };
            SmtpClient smtp = new(emailConfig.SmtpHost,
                emailConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(
                    emailConfig.CredUserName,
                    emailConfig.CredPassword),
                EnableSsl = emailConfig.EnableSsl
            };

            Logger.DebugPrepareToEnter(_logger, nameof(SmtpClient.SendMailAsync));
            Logger.DebugExitingMethod(_logger, nameof(SendVerificationMessageAsync));

            await smtp.SendMailAsync(m);
        }

        public async Task<bool> ConfirmUserContactMethod(string userId, string token)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(ConfirmUserContactMethod));
            Logger.DebugPrepareToEnter(_logger, nameof(_userManager.FindByIdAsync));
            var user = await _userManager.FindByIdAsync(userId) ??
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            Logger.DebugPrepareToEnter(_logger, nameof(_userManager.ConfirmEmailAsync));
            var result = await _userManager.ConfirmEmailAsync(user, token);

            Logger.InfoBoolResult(_logger, nameof(ConfirmUserContactMethod), result.Succeeded.ToString());
            Logger.DebugExitingMethod(_logger, nameof(ConfirmUserContactMethod));

            return result.Succeeded;
        }
    }
}