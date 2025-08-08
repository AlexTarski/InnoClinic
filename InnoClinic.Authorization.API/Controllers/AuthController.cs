using System.Net;
using System.Net.Mail;
using IdentityServer4;
using IdentityServer4.Services;
using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            ModelState.AddModelError(string.Empty, "User not found");
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

        ModelState.AddModelError(string.Empty, "Login error");
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
                Header = "Verification success",
                Message = "Thank you for confirming your account.",
            };
            
            return View("Message", successMessage);
        }

        var unexpectedErrorMessage = new MessageViewModel()
        {
            Title = "Unexpected Error",
            Header = "Unexpected error occurred",
            Message = "An error occurred during the confirmation process. Please contact the administrator for more information.",
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
        
        MailAddress from = new MailAddress("somemail@gmail.com", "no-reply-InnoClinic");
        // кому отправляем
        MailAddress to = new MailAddress("alex.f.l.o.w@yandex.ru"); //new MailAddress(user.Email);
        // создаем объект сообщения
        MailMessage m = new MailMessage(from, to);
        // тема письма
        m.Subject = "Email verification link";
        // текст письма
        m.Body = $"Please confirm your InnoClinic account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
        // письмо представляет код html
        m.IsBodyHtml = true;
        // адрес smtp-сервера и порт, с которого будем отправлять письмо
        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        // логин и пароль
        smtp.Credentials = new NetworkCredential("aliaksei.tarski@innowise.com", "tvsgrafuydydxpiq");
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(m);
    }
}