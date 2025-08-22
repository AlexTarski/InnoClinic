using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Authorization.Business.Configuration;

namespace InnoClinic.Authorization.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Account> _userManager;


        public EmailService(IConfiguration configuration, UserManager<Account> userManager)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} cannot be null");
            _userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager), $"{nameof(userManager)} cannot be null");
        }

        public async Task SendVerificationMessageAsync(string userAddress, string confirmationLink)
        {
            MailAddress from = new(_configuration["EmailSettings:From"], _configuration["EmailSettings:DisplayName"]);
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
            SmtpClient smtp = new(_configuration["EmailSettings:SmtpHost"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]))
            {
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:CredUserName"],
                    _configuration["EmailSettings:CredPassword"]),
                EnableSsl = true
            };

            await smtp.SendMailAsync(m);
        }

        public async Task<bool> ConfirmUserContactMethod(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }
    }
}