using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using AutoMapper;
using IdentityServer4;
using IdentityServer4.Services;

using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Authorization.Business;
using InnoClinic.Authorization.Business.Interfaces;

namespace InnoClinic.Authorization.API.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly IAccountService _accountService;

    public AuthController(SignInManager<Account> signInManager,
        UserManager<Account> userManager,
        IIdentityServerInteractionService interactionService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IMapper mapper,
        IMessageService messageService,
        IAccountService accountService) =>
        (_signInManager, _userManager, _interactionService, _httpClientFactory, _configuration, _mapper, _messageService, _accountService) =
        (signInManager ?? throw new ArgumentNullException(nameof(signInManager), $"{nameof(signInManager)} can not be null"),
        userManager ?? throw new ArgumentNullException(nameof(userManager), $"{nameof(userManager)} can not be null"),
        interactionService ?? throw new ArgumentNullException(nameof(interactionService), $"{nameof(interactionService)} can not be null"),
        httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory), $"{nameof(httpClientFactory)} can not be null"),
        configuration ?? throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} can not be null"),
        mapper ?? throw new ArgumentNullException(nameof(mapper), $"{nameof(mapper)} can not be null"),
        messageService ?? throw new ArgumentNullException(nameof(messageService), $"{nameof(messageService)} can not be null"),
        accountService ?? throw new ArgumentNullException(nameof(accountService), $"{nameof(accountService)} can not be null"));

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

        var viewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        var clientIdResult = await _accountService.GetClientIdAsync(returnUrl);

        if (!clientIdResult.IsSuccess)
        {
            return View("Message", clientIdResult.ErrorMessage);
        }

        viewModel.ClientId = clientIdResult.ClientId;
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var clientIdResult = await _accountService.GetClientIdAsync(viewModel.ReturnUrl);

        if (!clientIdResult.IsSuccess)
        {
            return View("Message", clientIdResult.ErrorMessage);
        }

        viewModel.ClientId = clientIdResult.ClientId;

        var user = await _userManager.FindByEmailAsync(viewModel.Email);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
            return View(viewModel);
        }

        if (clientIdResult.ClientId == ClientType.EmployeeUI.GetStringValue())
        {
            var profileType = await _accountService.GetProfileTypeAsync(user.Id);

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

            if (profileType == ProfileType.Doctor && !await _accountService.IsDoctorProfileActiveAsync(user.Id))
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

        if (await _accountService.IsEmailExistsAsync(viewModel.Email))
        {
            ModelState.AddModelError(nameof(viewModel.Email), "Someone already uses this email");
            return View(viewModel);
        }

        var user = _mapper.Map<RegisterViewModel, Account>(viewModel);

        var createResult = await _userManager.CreateAsync(user, viewModel.Password);

        if (!createResult.Succeeded)
        {
            BindErrorsToViewModel(createResult);
            return View(viewModel);
        }

        var updateResult = await _accountService.UpdateSelfCreatedUserAsync(user);

        if (!updateResult.Succeeded)
        {
            BindErrorsToViewModel(updateResult);
            return View(viewModel);
        }

        await _signInManager.SignInAsync(user, false);
        await SendVerificationEmailAsync(user);
        return RedirectToAction(nameof(RegistrationSuccess), ControllerContext.ActionDescriptor.ControllerName);
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
        try
        {
            var confirmed = await _messageService.ConfirmUserContactMethod(userId, token);

            if (confirmed)
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
        catch (KeyNotFoundException)
        {
            var errorMessage = new MessageViewModel()
            {
                Title = "Verification failed",
                Header = "User not found",
                Message = "User with this ID not found. Please, contact the administrator for more information.",
            };
            
            return View("Message", errorMessage);
        }        
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

        await _messageService.SendVerificationMessageAsync(user.Email, confirmationLink);
    }
}