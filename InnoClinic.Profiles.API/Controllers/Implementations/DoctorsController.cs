using AutoMapper;

using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.DataSeeding.Entities;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers.Implementations;

[ApiController]
[Route("api/[Controller]")]
public class DoctorsController : BaseUserController<Doctor, DoctorModel>
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(ILogger<DoctorsController> logger,
        IDoctorService service,
        IMapper mapper) : base(logger, service, mapper)
    {
        _doctorService = service ?? throw new DiNullReferenceException(nameof(service));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctorsAsync()
    {
        return await GetAllAsync();
    }

    //TODO: review this endpoint
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetDoctorByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    //TODO: review this endpoint
    [HttpGet("accounts/{accountId:Guid}")]
    public async Task<IActionResult> CheckDoctorExistsByAccountIdAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{accountId:Guid}/status")]
    public async Task<IActionResult> CheckDoctorStatusAsync(Guid accountId)
    {
        try
        {
            var isActive = await _doctorService.IsProfileActiveAsync(accountId);
            return isActive ? Ok("This profile is active") 
                : StatusCode(403, "This profile is inactive and cannot be used to access services.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Failed to check by account ID {AccountId}", accountId);
            return NotFound($"Doctor with this account ID was not found");
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
            var success = await _doctorService.DeactivateProfilesByOfficeIdAsync(officeId);
            return success ? NoContent() : StatusCode(500, $"Failed to deactivate {nameof(Doctor)}s profiles");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"No {nameof(Doctor)} profiles were found for this {nameof(Office)} ID");
        }
    }

    //TODO: review this endpoint
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteDoctorAsync(Guid id)
    {
        return await DeleteAsync(id);
    }

    [HttpGet("/secret")]
    [Authorize]
    public IActionResult GetSecret()
    {
        return Ok("This is a secret message only for authorized users.");
    }
}