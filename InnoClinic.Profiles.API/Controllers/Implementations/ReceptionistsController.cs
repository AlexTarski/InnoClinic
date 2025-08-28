using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers.Implementations;

[ApiController]
[Route("api/[Controller]")]
public class ReceptionistsController : BaseUserController<Receptionist, ReceptionistModel>
{
    public ReceptionistsController(ILogger<ReceptionistsController> logger,
        IReceptionistService service,
        IMapper mapper) : base(logger, service, mapper) { }

    [HttpGet]
    public async Task<IActionResult> GetAllReceptionistsAsync()
    {
        return await GetAllAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetReceptionistByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }
    
    [HttpGet("accounts/{accountId:Guid}")]
    public async Task<IActionResult> DoctorExistsByAccountIdAsync(Guid accountId)
    {
        return await CheckUserExistsAsync(accountId);
    }

    [HttpPost]
    public async Task<IActionResult> AddReceptionistAsync([FromBody] ReceptionistModel model)
    {
        return await AddAsync(model);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteReceptionistAsync(Guid id)
    {
        return await DeleteAsync(id);
    }
}