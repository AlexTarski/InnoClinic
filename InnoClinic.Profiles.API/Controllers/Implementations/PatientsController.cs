using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers.Implementations;

[ApiController]
[Route("api/[Controller]")]
public class PatientsController : BaseUserController<Patient, PatientModel>
{
    public PatientsController(ILogger<PatientsController> logger,
        IPatientService service,
        IMapper mapper) : base(logger, service, mapper) { }

    [HttpGet]
    public async Task<IActionResult> GetAllPatientsAsync()
    {
        return await GetAllAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetPatientByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }
    
    [HttpGet("accounts/{accountId:Guid}")]
    public async Task<IActionResult> PatientExistsByAccountIdAsync(Guid accountId)
    {
        return await CheckUserExistsAsync(accountId);
    }

    [HttpPost]
    public async Task<IActionResult> AddPatientAsync([FromBody] PatientModel model)
    {
        return await AddAsync(model);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeletePatientAsync(Guid id)
    {
        return await DeleteAsync(id);
    }
}