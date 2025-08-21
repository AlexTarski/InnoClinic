using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using AutoMapper;
using IdentityServer4;
using IdentityServer4.Services;

using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Authorization.Business;

namespace InnoClinic.Authorization.API.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthController(SignInManager<Account> signInManager,
        UserManager<Account> userManager,
        IIdentityServerInteractionService interactionService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IMapper mapper) =>
        (_signInManager, _userManager, _interactionService, _httpClientFactory, _configuration, _mapper) =
        (signInManager, userManager, interactionService,  httpClientFactory, configuration, mapper);

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        if (returnUrl == null)
        {
            var errorMessage = new MessageViewModel
            {
                Title = "Error",
                Header = "Invalid login page access",
                Message = "Please, contact the administrator for more information."
            };

            return View("Message", errorMessage);
        }

        var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);
        var viewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        if (context != null)
        {
            var clientId = context.Client?.ClientId;

            if (clientId == null)
            {
                var errorMessage = new MessageViewModel
                {
                    Title = "Error",
                    Header = "Invalid Client",
                    Message = "The Client ID is not valid or not provided."
                };

                return View("Message", errorMessage);
            }

            viewModel.ClientId = clientId;
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var context = await _interactionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
        if (context == null)
        {
            var errorMessage = new MessageViewModel
            {
                Title = "Error",
                Header = "Invalid login page access",
                Message = "Please, contact the administrator for more information."
            };
            return View("Message", errorMessage);
        }

        var clientId = context.Client?.ClientId;
        if (clientId == null)
        {
            var errorMessage = new MessageViewModel
            {
                Title = "Error",
                Header = "Invalid Client",
                Message = "The Client ID is not valid or not provided."
            };

            return View("Message", errorMessage);
        }

        viewModel.ClientId = clientId;

        var user = await _userManager.FindByEmailAsync(viewModel.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
            return View(viewModel);
        }

        if (clientId == ClientType.EmployeeUI.GetStringValue())
        {
            var profileType = await GetProfileTypeAsync(user.Id);
            if (profileType == ProfileType.Patient || profileType == ProfileType.UnknownProfile)
            {
                var errorMessage = new MessageViewModel
                {
                    Title = "Login Error",
                    Header = "Invalid Profile Type",
                    Message = "Only employees can access employee services."
                };
                
                return View("Message", errorMessage);
            }

            if (profileType == ProfileType.Doctor && !await IsDoctorProfileActiveAsync(user.Id))
            {
                    ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
                    return View(viewModel);
            }
        }

        var result = await _signInManager.PasswordSignInAsync(user,
            viewModel.Password, false, false);

        if(result.Succeeded)
        {
            var identityServerUser = new IdentityServerUser(user.Id.ToString())
            {
                DisplayName = user.Email,
                IdentityProvider = _configuration["IdentityProviders:Local"],
                AuthenticationTime = DateTime.UtcNow
            };

            await HttpContext.SignInAsync(identityServerUser);
            return Redirect(viewModel.ReturnUrl);
        }

        ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Register(string returnUrl)
    {
        var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);
        var clientId = context?.Client?.ClientId;

        if (clientId != ClientType.ClientUI.GetStringValue())
        {
            var errorMessage = new MessageViewModel
            {
                Title = "Access Denied",
                Header = "Unauthorized Client",
                Message = "This endpoint is not accessible from your application."
            };

            return View("Message", errorMessage);
        }

        var viewModel = new RegisterViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if(await IsEmailExists(viewModel))
            return View(viewModel);

        var user = _mapper.Map<RegisterViewModel, Account>(viewModel);

        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (result.Succeeded)
        {
            user.CreatedBy = user.Id;
            user.UpdatedBy = user.Id;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, false);
            await SendVerificationEmailAsync(user);
            return RedirectToAction(nameof(RegistrationSuccess), ControllerContext.ActionDescriptor.ControllerName);
        }
        else
        {
            if(result == null)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred. Contact administrator.");
                return View(viewModel);
            }

            BindErrorsToViewModel(result);
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
        return Redirect(logoutRequest.PostLogoutRedirectUri);
    }

    [HttpGet]
    public IActionResult RegistrationSuccess()
    {
        var successMessage = new MessageViewModel()
        {
            Title = "Registration complete!",
            Header = "Registration process complete successfully!",
            Message = "Thanks for signing up! Please check your email to confirm your account."
        };
        
        return View("Message", successMessage);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            var errorMessage = new MessageViewModel()
            {
                Title = "Verification failed",
                Header = "User not found",
                Message = "User with this ID not found. Please, contact the administrator for more information.",
            };
            
            return View("Message", errorMessage);
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        if (result.Succeeded)
        {
            var successMessage = new MessageViewModel()
            {
                Title = "Verification success",
                Header = "Email verification success",
                Message = "Thank you for confirming your account!",
                IsEmailVerificationSuccessMessage = true
            };
            
            return View("Message", successMessage);
        }

        var unexpectedErrorMessage = new MessageViewModel()
        {
            Title = "Unexpected Error",
            Header = "Unexpected error occurred",
            Message = "An error occurred during the confirmation process. Please contact the administrator for more information."
        };

        return View("Message", unexpectedErrorMessage);
    }

    private async Task<bool> IsEmailExists(RegisterViewModel viewModel)
    {
        if (await _userManager.FindByEmailAsync(viewModel.Email) != null)
        {
            ModelState.AddModelError(nameof(viewModel.Email), "Someone already uses this email");
            return true;
        }

        return false;
    }

    private void BindErrorsToViewModel(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    private async Task SendVerificationEmailAsync(Account user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), ControllerContext.ActionDescriptor.ControllerName,
            new { userId = user.Id, token = token }, Request.Scheme);
        
        MailAddress from = new(_configuration["EmailSettings:From"], _configuration["EmailSettings:DisplayName"]);
        MailAddress to = new($"{user.Email}");
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
        SmtpClient smtp = new(
            _configuration["EmailSettings:SmtpHost"],
            int.Parse(_configuration["EmailSettings:SmtpPort"]))
        {
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:CredUserName"],
                _configuration["EmailSettings:CredPassword"]),
            EnableSsl = true
        };

        await smtp.SendMailAsync(m);
    }

    private async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var result = await httpClient
            .GetAsync($"{AppUrls.ProfilesUrl}/api/Patients/accounts/{accountId}");
        
        if (result.IsSuccessStatusCode)
        {
            return ProfileType.Patient;
        }
        
        result = await httpClient
            .GetAsync($"{AppUrls.ProfilesUrl}/api/Doctors/accounts/{accountId}");

        if (result.IsSuccessStatusCode)
        {
            return ProfileType.Doctor;
        }
        
        result = await httpClient
            .GetAsync($"{AppUrls.ProfilesUrl}/api/Receptionists/accounts/{accountId}");

        if (result.IsSuccessStatusCode)
        {
            return ProfileType.Receptionist;
        }
        
        return ProfileType.UnknownProfile;
    }
    
    private async Task<bool> IsDoctorProfileActiveAsync(Guid accountId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var result = await httpClient
            .GetAsync($"{AppUrls.ProfilesUrl}/api/Doctors/{accountId}/status");
        
        return result.IsSuccessStatusCode;
    }
}