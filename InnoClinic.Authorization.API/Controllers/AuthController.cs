using AutoMapper;
using IdentityServer4;
using IdentityServer4.Services;
using InnoClinic.Authorization.API.Configurations;
using InnoClinic.Authorization.Business;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.API.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly IAccountService _accountService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(SignInManager<Account> signInManager,
        UserManager<Account> userManager,
        IIdentityServerInteractionService interactionService,
        IMapper mapper,
        IMessageService messageService,
        IAccountService accountService,
        ILogger<AuthController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null");
        _signInManager = signInManager ?? HandleDiNullReference<SignInManager<Account>>(nameof(signInManager));
        _userManager = userManager ?? HandleDiNullReference<UserManager<Account>>(nameof(userManager));
        _interactionService = interactionService ?? HandleDiNullReference<IIdentityServerInteractionService>(nameof(interactionService));
        _mapper = mapper ?? HandleDiNullReference<IMapper>(nameof(mapper));
        _messageService = messageService ?? HandleDiNullReference<IMessageService>(nameof(messageService));
        _accountService = accountService ?? HandleDiNullReference<IAccountService>(nameof(accountService));
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Login));

        if (returnUrl == null)
        {
            Logger.WarningInvalidLoginPageAccess(_logger);
            var errorMessage = new MessageViewModel
            {
                Title = "Error",
                Header = "Invalid login page access",
                Message = "Please, contact the administrator for more information."
            };

            Logger.InfoSendInfoPageToClient(_logger, errorMessage.Header);
            return View("Message", errorMessage);
        }

        var viewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };
        
        var clientIdResult = await _accountService.GetClientIdAsync(returnUrl);

        if (!clientIdResult.IsSuccess)
        {
            Logger.WarningInvalidLoginPageAccess(_logger);
            Logger.InfoSendInfoPageToClient(_logger, clientIdResult.ErrorMessage.Header);
            return View("Message", clientIdResult.ErrorMessage);
        }

        viewModel.ClientId = clientIdResult.ClientId;
        Logger.DebugExitingMethod(_logger, nameof(Login));
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Login));

        if (!ModelState.IsValid)
        {
            Logger.WarningFailedToSignIn(_logger);
            return View(viewModel);
        }

        var clientIdResult = await _accountService.GetClientIdAsync(viewModel.ReturnUrl);

        if (!clientIdResult.IsSuccess)
        {
            Logger.WarningFailedToSignIn(_logger);
            Logger.InfoSendInfoPageToClient(_logger, clientIdResult.ErrorMessage.Header);
            return View("Message", clientIdResult.ErrorMessage);
        }

        viewModel.ClientId = clientIdResult.ClientId;

        Logger.DebugExecutingMethod(_logger, nameof(_userManager.FindByEmailAsync));
        var user = await _userManager.FindByEmailAsync(viewModel.Email);

        if (user == null)
        {
            Logger.WarningFailedToSignIn(_logger);
            ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
            return View(viewModel);
        }

        if (clientIdResult.ClientId == ClientType.EmployeeUI.GetStringValue())
        {
            var profileType = await _accountService.GetProfileTypeAsync(user.Id);

            if (profileType == ProfileType.Patient || profileType == ProfileType.UnknownProfile)
            {
                Logger.WarningFailedToSignIn(_logger);
                var errorMessage = new MessageViewModel
                {
                    Title = "Login Error",
                    Header = "Invalid Profile Type",
                    Message = "Only employees can access employee services."
                };

                Logger.InfoSendInfoPageToClient(_logger, errorMessage.Header);
                return View("Message", errorMessage);
            }

            if (profileType == ProfileType.Doctor && !await _accountService.IsDoctorProfileActiveAsync(user.Id))
            {
                Logger.WarningFailedToSignIn(_logger);
                ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
                return View(viewModel);
            }
        }

        Logger.InfoTrySignIn(_logger);
        var result = await _signInManager.PasswordSignInAsync(user,
            viewModel.Password, false, false);

        if (result.Succeeded)
        {
            var identityServerUser = new IdentityServerUser(user.Id.ToString())
            {
                DisplayName = user.Email,
                IdentityProvider = IdentityProvider.Local.GetStringValue(),
                AuthenticationTime = DateTime.UtcNow
            };

            await HttpContext.SignInAsync(identityServerUser);
            Logger.InfoSignInSuccess(_logger);
            return Redirect(viewModel.ReturnUrl);
        }

        Logger.WarningFailedToSignIn(_logger);
        ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
        Logger.DebugExitingMethod(_logger, nameof(Login));
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Register(string returnUrl)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Register));
        var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);
        var clientId = context?.Client?.ClientId;

        if (clientId != ClientType.ClientUI.GetStringValue())
        {
            Logger.WarningInvalidRegisterPageAccess(_logger);
            var errorMessage = new MessageViewModel
            {
                Title = "Access Denied",
                Header = "Unauthorized Client",
                Message = "This endpoint is not accessible from your application."
            };

            Logger.InfoSendInfoPageToClient(_logger, errorMessage.Header);
            return View("Message", errorMessage);
        }

        var viewModel = new RegisterViewModel
        {
            ReturnUrl = returnUrl
        };

        Logger.DebugExitingMethod(_logger, nameof(Register));
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Register));
        if (!ModelState.IsValid)
        {
            Logger.WarningFailedToSignUp(_logger);
            return View(viewModel);
        }

        if (await _accountService.IsEmailExistsAsync(viewModel.Email))
        {
            Logger.WarningFailedToSignUp(_logger);
            ModelState.AddModelError(nameof(viewModel.Email), "Someone already uses this email");
            return View(viewModel);
        }

        var user = _mapper.Map<RegisterViewModel, Account>(viewModel);

        var createResult = await _userManager.CreateAsync(user, viewModel.Password);

        if (!createResult.Succeeded)
        {
            Logger.WarningFailedToSignUp(_logger);
            BindErrorsToViewModel(createResult);
            return View(viewModel);
        }

        Logger.InfoTrySignUp(_logger);
        var updateResult = await _accountService.UpdateSelfCreatedUserAsync(user);

        if (!updateResult.Succeeded)
        {
            Logger.WarningFailedToSignUp(_logger);
            BindErrorsToViewModel(updateResult);
            return View(viewModel);
        }

        Logger.InfoTrySignIn(_logger);
        await _signInManager.SignInAsync(user, false);
        Logger.DebugExecutingMethod(_logger, nameof(SendVerificationEmailAsync));
        await SendVerificationEmailAsync(user);
        Logger.InfoSignUpSuccess(_logger);
        Logger.DebugExitingMethod(_logger, nameof(Register));
        return RedirectToAction(nameof(RegistrationSuccess), ControllerContext.ActionDescriptor.ControllerName);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Logout));
        Logger.InfoTrySignOut(_logger);
        await _signInManager.SignOutAsync();
        var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
        Logger.InfoSignOutSuccess(_logger);
        Logger.DebugExitingMethod(_logger, nameof(Logout));
        return Redirect(logoutRequest.PostLogoutRedirectUri);
    }

    [HttpGet]
    public IActionResult RegistrationSuccess()
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(RegistrationSuccess));
        var successMessage = new MessageViewModel()
        {
            Title = "Registration complete!",
            Header = "Registration process complete successfully!",
            Message = "Thanks for signing up! Please check your email to confirm your account."
        };

        Logger.InfoSendInfoPageToClient(_logger, successMessage.Header);
        Logger.DebugExitingMethod(_logger, nameof(RegistrationSuccess));
        return View("Message", successMessage);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        try
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(ConfirmEmail));
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

                Logger.InfoEmailVerificationSuccess(_logger);
                Logger.InfoSendInfoPageToClient(_logger, successMessage.Header);
                return View("Message", successMessage);
            }

            var unexpectedErrorMessage = new MessageViewModel()
            {
                Title = "Unexpected Error",
                Header = "Unexpected error occurred",
                Message = "An error occurred during the confirmation process. Please contact the administrator for more information."
            };

            LogEmailVerificationFail(unexpectedErrorMessage.Header);
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

            LogEmailVerificationFail(errorMessage.Header);
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
        Logger.DebugStartProcessingMethod(_logger, nameof(SendVerificationEmailAsync));
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), ControllerContext.ActionDescriptor.ControllerName,
            new { userId = user.Id, token = token }, Request.Scheme);

        await _messageService.SendVerificationMessageAsync(user.Email, confirmationLink);
    }

    private T HandleDiNullReference<T>(string dependency)
    {
        var exception = new ArgumentNullException(dependency, $"{dependency} cannot be null");
        Logger.CriticalDiNullReference(_logger, exception, exception.Message);
        throw exception;
    }

    private void LogEmailVerificationFail(string errorMessage)
    {
        Logger.WarningEmailVerificationFailed(_logger);
        Logger.InfoSendInfoPageToClient(_logger, errorMessage);
        Logger.DebugExitingMethod(_logger, nameof(ConfirmEmail));
    }
}