using AutoMapper;

using InnoClinic.Profiles.Business.Filters;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers.Implementations;

[ApiController]
[Route("api/[Controller]")]
public class ReceptionistsController : BaseUserController<Receptionist, ReceptionistParameters, ReceptionistModel>
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

    [HttpGet("accountId/{accountId:Guid}")]
    public async Task<IActionResult> GetReceptionistByAccountIdAsync(Guid accountId)
    {
        return await GetByAccountIdAsync(accountId);
    }

    //TODO: review this endpoint
    [HttpPost]
    public async Task<IActionResult> AddReceptionistAsync([FromBody] ReceptionistModel model)
    {
        return await AddAsync(model);
    }

    //TODO: review this endpoint
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteReceptionistAsync(Guid id)
    {
        return await DeleteAsync(id);
    }
}