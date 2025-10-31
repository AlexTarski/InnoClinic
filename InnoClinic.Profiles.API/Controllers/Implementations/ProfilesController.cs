using Microsoft.AspNetCore.Mvc;

using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Profiles.API.Controllers.Implementations
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
                throw new DiNullReferenceException(nameof(profilesService));
            _logger = logger ??
                throw new DiNullReferenceException(nameof(logger));
        }

        [HttpGet("{accountId:Guid}/type")]
        public async Task<IActionResult> GetProfileTypeAsync(Guid accountId)
        {
            var profileType = await _profilesService.GetProfileTypeAsync(accountId);
            return Ok(profileType.ToString());
        }
    }
}