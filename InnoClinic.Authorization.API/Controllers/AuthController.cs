using AutoMapper;

using IdentityServer4;
using IdentityServer4.Services;

using InnoClinic.Authorization.API.Configurations;
using InnoClinic.Authorization.Business;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

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
        _logger = logger ??
            throw new DiNullReferenceException(nameof(logger));
        _signInManager = signInManager ??
            throw new DiNullReferenceException(nameof(signInManager));
        _userManager = userManager ??
            throw new DiNullReferenceException(nameof(userManager));
        _interactionService = interactionService ??
            throw new DiNullReferenceException(nameof(interactionService));
        _mapper = mapper ??
            throw new DiNullReferenceException(nameof(mapper));
        _messageService = messageService ??
            throw new DiNullReferenceException(nameof(messageService));
        _accountService = accountService ??
            throw new DiNullReferenceException(nameof(accountService));
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Login));

        if (returnUrl == null)
        {
            var errorMessage = new MessageViewModel
            {
                Title = "Error",
                Header = "Invalid login page access",
                Message = "Please, contact the administrator for more information."
            };

            LogInvalidPageAccess(nameof(Login), errorMessage.Header);

            return View("Message", errorMessage);
        }

        var viewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };
        
        var clientIdResult = await _accountService.GetClientIdAsync(returnUrl);

        if (!clientIdResult.IsSuccess)
        {
            LogInvalidPageAccess(nameof(Login), clientIdResult.ErrorMessage.Header);

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
            LogMethodExit(Logger.WarningFailedDoAction, nameof(Login));

            return View(viewModel);
        }

        var clientIdResult = await _accountService.GetClientIdAsync(viewModel.ReturnUrl);

        if (!clientIdResult.IsSuccess)
        {
            Logger.WarningFailedDoAction(_logger, nameof(Login));
            Logger.InfoSendInfoPageToClient(_logger, clientIdResult.ErrorMessage.Header);

            return View("Message", clientIdResult.ErrorMessage);
        }

        viewModel.ClientId = clientIdResult.ClientId;

        Logger.DebugPrepareToEnter(_logger, nameof(_userManager.FindByEmailAsync));
        var user = await _userManager.FindByEmailAsync(viewModel.Email);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
            LogMethodExit(Logger.WarningFailedDoAction, nameof(Login));

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

                Logger.WarningFailedDoAction(_logger, nameof(Login));
                Logger.InfoSendInfoPageToClient(_logger, errorMessage.Header);

                return View("Message", errorMessage);
            }

            if (profileType == ProfileType.Doctor && !await _accountService.IsDoctorProfileActiveAsync(user.Id))
            {
                ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
                LogMethodExit(Logger.WarningFailedDoAction, nameof(Login));

                return View(viewModel);
            }
        }

        Logger.InfoTryDoAction(_logger, nameof(Login));
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
            LogMethodExit(Logger.InfoSuccess, nameof(Login));

            return Redirect(viewModel.ReturnUrl);
        }

        ModelState.AddModelError(string.Empty, "Either an email or a password is incorrect");
        LogMethodExit(Logger.WarningFailedDoAction, nameof(Login));

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
            var errorMessage = new MessageViewModel
            {
                Title = "Access Denied",
                Header = "Unauthorized Client",
                Message = "This endpoint is not accessible from your application."
            };

            LogInvalidPageAccess(nameof(Register), errorMessage.Header);

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
            LogMethodExit(Logger.WarningFailedDoAction, nameof(Register));

            return View(viewModel);
        }

        if (await _accountService.IsEmailExistsAsync(viewModel.Email))
        {
            ModelState.AddModelError(nameof(viewModel.Email), "Someone already uses this email");
            LogMethodExit(Logger.WarningFailedDoAction, nameof(Register));

            return View(viewModel);
        }

        var user = _mapper.Map<RegisterViewModel, Account>(viewModel);

        var createResult = await _userManager.CreateAsync(user, viewModel.Password);

        if (!createResult.Succeeded)
        {
            BindErrorsToViewModel(createResult);
            LogMethodExit(Logger.WarningFailedDoAction, nameof(Register));

            return View(viewModel);
        }

        Logger.InfoTryDoAction(_logger, nameof(Register));
        var updateResult = await _accountService.UpdateSelfCreatedUserAsync(user);

        if (!updateResult.Succeeded)
        {
            BindErrorsToViewModel(updateResult);
            LogMethodExit(Logger.WarningFailedDoAction, nameof(Register));

            return View(viewModel);
        }

        Logger.InfoTryDoAction(_logger, nameof(Login));
        await _signInManager.SignInAsync(user, false);
        Logger.DebugPrepareToEnter(_logger, nameof(SendVerificationEmailAsync));
        await SendVerificationEmailAsync(user);
        LogMethodExit(Logger.InfoSuccess, nameof(Register));

        return RedirectToAction(nameof(RegistrationSuccess), ControllerContext.ActionDescriptor.ControllerName);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(Logout));
        Logger.InfoTryDoAction(_logger, nameof(Logout));
        await _signInManager.SignOutAsync();
        var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
        LogMethodExit(Logger.InfoSuccess, nameof(Logout));

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

                Logger.InfoSuccess(_logger, nameof(ConfirmEmail));
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
        Logger.DebugStartProcessingMethod(_logger, nameof(BindErrorsToViewModel));
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        Logger.DebugExitingMethod(_logger, nameof(BindErrorsToViewModel));
    }

    private async Task SendVerificationEmailAsync(Account user)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(SendVerificationEmailAsync));
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), ControllerContext.ActionDescriptor.ControllerName,
            new { userId = user.Id, token = token }, Request.Scheme);

        await _messageService.SendVerificationMessageAsync(user.Email, confirmationLink);
        Logger.DebugExitingMethod(_logger, nameof(SendVerificationEmailAsync));
    }

    private void LogEmailVerificationFail(string errorMessage)
    {
        Logger.WarningFailedDoAction(_logger, nameof(ConfirmEmail));
        Logger.InfoSendInfoPageToClient(_logger, errorMessage);
        Logger.DebugExitingMethod(_logger, nameof(ConfirmEmail));
    }

    private void LogInvalidPageAccess(string pageName, string errorMessage)
    {
        Logger.WarningInvalidPageAccess(_logger, pageName);
        Logger.InfoSendInfoPageToClient(_logger, errorMessage);
    }

    /// <summary>
    /// Logs the exit of a method using the specified logging action and method name.
    /// Use <see cref="Logger.InfoSuccess"/> for successful method execution,
    /// and <see cref="Logger.WarningFailedDoAction"/> if execution failed.
    /// </summary>
    /// <param name="logMethod">
    /// The logging action to execute, which takes an <see cref="ILogger{TCategoryName}"/> instance
    /// and the name of the method being exited.
    /// </param>
    /// <param name="methodName">
    /// The name of the method that is exiting. This value is included in the log entry.
    /// </param>
    private void LogMethodExit(Action<ILogger<AuthController>, string> logMethod, string methodName)
    {
        logMethod(_logger, methodName);
        Logger.DebugExitingMethod(_logger, methodName);
    }
}