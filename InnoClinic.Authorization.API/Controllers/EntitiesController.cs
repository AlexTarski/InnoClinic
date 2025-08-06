using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Business.Models.UserModels;
using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class EntitiesController : BaseController<YourEntity, YourEntityModel>
{
    public EntitiesController(ILogger<EntitiesController> logger,
        IYourEntityService service,
        IMapper mapper) : base(logger, service, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllYourEntitiesAsync()
    {
        return await GetAllAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetYourEntityByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> AddYourEntityAsync([FromBody] YourEntityModel model)
    {
        return await AddAsync(model);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteYourEntityAsync(Guid id)
    {
        return await DeleteAsync(id);
    }
}