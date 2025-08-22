using Microsoft.AspNetCore.Mvc;

using InnoClinic.Profiles.Business.Interfaces;

namespace InnoClinic.Profiles.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfilesService _profilesService;
        private readonly ILogger<ProfilesController> _logger;

        public ProfilesController(IProfilesService profilesService, ILogger<ProfilesController> logger)
        {
            _profilesService = profilesService ??
                throw new ArgumentNullException(nameof(profilesService), $"{nameof(profilesService)} cannot be null");
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null");
        }

        [HttpGet("{accountId:Guid}/type")]
        public async Task<IActionResult> GetProfileTypeAsync(Guid accountId)
        {
            try
            {
                var profileType = await _profilesService.GetProfileTypeAsync(accountId);
                return Ok(profileType.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting profile type for account ID {AccountId}", accountId);
                return StatusCode(500, "An error occurred while attempting to retrieve the profile type.");
            }
        }
    }
}