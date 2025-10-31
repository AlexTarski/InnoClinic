using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountDataController : ControllerBase
    {
        private readonly ILogger<AccountDataController> _logger;
        private readonly IAccountService _accountService;

        public AccountDataController(ILogger<AccountDataController> logger, IAccountService accountService)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _accountService = accountService ?? throw new DiNullReferenceException(nameof(accountService));
        }

        [HttpGet("photoId/{accountId:guid}")]
        public async Task<IActionResult> GetPhotoIdAsync(Guid accountId)
        {
            try
            {
                var photoId = await _accountService.GetPhotoIdAsync(accountId);

                return Ok(photoId);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warning(_logger, ex, $"Failed to get photo ID by {typeof(Account).Name} ID: {accountId}");
                return NotFound($"{typeof(Account).Name} was not found");
            }
        }
    }
}