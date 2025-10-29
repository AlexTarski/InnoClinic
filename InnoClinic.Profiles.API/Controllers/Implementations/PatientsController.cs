using AutoMapper;

using InnoClinic.Profiles.Business.Filters;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers.Implementations;

[ApiController]
[Route("api/[Controller]")]
public class PatientsController : BaseUserController<Patient, PatientParameters, PatientModel>
{
    public PatientsController(ILogger<PatientsController> logger,
        IPatientService service,
        IMapper mapper) : base(logger, service, mapper) { }

    [HttpGet]
    public async Task<IActionResult> GetAllPatientsAsync([FromQuery] PatientParameters patientParameters)
    {
        return await GetAllFilteredAsync(patientParameters);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetPatientByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    [HttpGet("accountId/{accountId:Guid}")]
    public async Task<IActionResult> GetPatientByAccountIdAsync(Guid accountId)
    {
        return await GetByAccountIdAsync(accountId);
    }

    //TODO: review this endpoint
    [HttpPost]
    public async Task<IActionResult> AddPatientAsync([FromBody] PatientModel model)
    {
        return await AddAsync(model);
    }

    //TODO: review this endpoint
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeletePatientAsync(Guid id)
    {
        return await DeleteAsync(id);
    }
}