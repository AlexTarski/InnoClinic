using IdentityServer4;
using IdentityServer4.Services;
using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

namespace InnoClinic.Authorization.API.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;

    public AuthController(SignInManager<Account> signInManager,
        UserManager<Account> userManager,
        IIdentityServerInteractionService interactionService) =>
        (_signInManager, _userManager, _interactionService) =
        (signInManager, userManager, interactionService);

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        var viewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await _userManager.FindByEmailAsync(viewModel.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
            return View(viewModel);
        }

        var result = await _signInManager.PasswordSignInAsync(user,
            viewModel.Password, false, false);
        if (result.Succeeded)
        {
            var identityServerUser = new IdentityServerUser(user.Id.ToString())
            {
                DisplayName = user.Email,
                IdentityProvider = "local",
                AuthenticationTime = DateTime.UtcNow
            };

            await HttpContext.SignInAsync(identityServerUser);
            return Redirect(viewModel.ReturnUrl);
        }

        ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
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


        var user = new Account
        {
            Email = viewModel.Email,
            UserName = viewModel.Email,
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (result.Succeeded)
        {
            user.CreatedBy = user.Id;
            user.UpdatedBy = user.Id;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, false);
            await SendVerificationEmailAsync(user);
            return RedirectToAction("RegistrationSuccess", "Auth");
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
        if(result.Succeeded)
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
            ModelState.AddModelError("Email", "Someone already uses this email");
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
        var confirmationLink = Url.Action("ConfirmEmail", "Auth",
            new { userId = user.Id, token = token }, Request.Scheme);
        
        MailAddress from = new("somemail@gmail.com", "no-reply-InnoClinic");
        MailAddress to = new($"{user.Email}");
        MailMessage m = new(from, to)
        {
            Subject = "Email verification link",
            Body = $@"
                <div style='font-family:Segoe UI, sans-serif; font-size:16px; color:#333;'>
                <div style='text-align:center; margin-bottom:20px;'>
                <img src='https://localhost:10036/assets/innoclinic-logo.png' alt='InnoClinic Logo' style='height:60px;' />
                </div>
                <p>Thank you for registering with <strong>InnoClinic</strong>.</p>
                <p>Please confirm your InnoClinic account by clicking the link below:</p>
                <p><a href='{HtmlEncoder.Default.Encode(confirmationLink)}' style='color:#3498db;'>Confirm Email</a></p>
                <hr style='margin:20px 0; border:none; border-top:1px solid #ccc;' />
                <p style='font-size:14px; color:#777;'>© 2025 InnoClinic. All rights reserved.</p>
                <p style='font-size:14px;'>
                <a href='https://localhost:4200/' style='color:#777;'>InnoClinic</a> |
                <a href='https://innowise.com/' style='color:#777;'>Innowise</a> |
                <a href='https://innowise.com/careers/' style='color:#777;'>Careers</a> |
                <a href='https://innowise.com/contact-us/' style='color:#777;'>Contact Us</a>
                </p>
                </div>",
            IsBodyHtml = true
        };
        SmtpClient smtp = new("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("aliaksei.tarski@innowise.com", "tvsgrafuydydxpiq"),
            EnableSsl = true
        };

        await smtp.SendMailAsync(m);
    }
}