using AutoMapper;

using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.DataSeeding.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers.Implementations;

[ApiController]
[Route("api/[Controller]")]
public class DoctorsController : BaseUserController<Doctor, DoctorModel>
{
    public DoctorsController(ILogger<DoctorsController> logger, IDoctorService service,
        IMapper mapper)
        : base(logger, service, mapper){}

    [HttpGet]
    public async Task<IActionResult> GetAllDoctorsAsync()
    {
        return await GetAllAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetDoctorByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    [HttpGet("accountId/{accountId:Guid}")]
    public async Task<IActionResult> GetDoctorByAccountIdAsync(Guid accountId)
    {
        return await GetByAccountIdAsync(accountId);
    }

    [HttpGet("{accountId:Guid}/status")]
    public async Task<IActionResult> CheckDoctorStatusAsync(Guid accountId)
    {
        try
        {
            var doctorService = (IDoctorService)_service;
            var isActive = await doctorService.IsProfileActiveAsync(accountId);

            return isActive ? Ok("This profile is active") 
                : StatusCode(403, "This profile is inactive and cannot be used to access services.");
        }
        catch (KeyNotFoundException ex)
        {
            Logger.Warning(_logger, ex, $"Failed to check {nameof(Doctor)} status by account ID {accountId}");

            return NotFound($"{nameof(Doctor)} with this account ID was not found");
        }
    }

    //TODO: review this endpoint
    [HttpPost]
    public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorModel model)
    {
        return await AddAsync(model);
    }

    [Authorize(Roles = UserRoles.Receptionist)]
    [HttpPut("deactivate/byOfficeId/{officeId:Guid}")]
    public async Task<IActionResult> DeactivateDoctorsByOfficeIdAsync(Guid officeId)
    {
        try
        {
            var doctorService = (IDoctorService)_service;
            var success = await doctorService.DeactivateProfilesByOfficeIdAsync(officeId);

            return success ? NoContent() : StatusCode(500, $"Failed to deactivate {nameof(Doctor)}s profiles");
        }
        catch (KeyNotFoundException ex)
        {
            Logger.Warning(_logger, ex, $"Failed to deactivate {nameof(Doctor)} profile by {nameof(Office)} ID {officeId}");

            return NotFound($"No {nameof(Doctor)} profiles were found for this {nameof(Office)} ID");
        }
    }

    //TODO: review this endpoint
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteDoctorAsync(Guid id)
    {
        return await DeleteAsync(id);
    }

    //TODO: debug method, delete before release
    [HttpGet("/secret")]
    [Authorize]
    public IActionResult GetSecret()
    {
        return Ok("This is a secret message only for authorized users.");
    }
}