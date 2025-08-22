using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class DoctorsController : BaseController<Doctor, DoctorModel>
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(ILogger<DoctorsController> logger,
        IDoctorService service,
        IMapper mapper) : base(logger, service, mapper)
    {
        _doctorService = service ?? throw new ArgumentNullException(nameof(service), "Service cannot be null");
    }

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

    [HttpGet("accounts/{accountId:Guid}")]
    public async Task<IActionResult> CheckDoctorExistsByAccountIdAsync(Guid accountId)
    {
        return await CheckUserExistsAsync(accountId);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check by account ID {AccountId}", accountId);
            return StatusCode(500, "Failed to check by account ID: Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorModel model)
    {
        return await AddAsync(model);
    }

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