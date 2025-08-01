using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models;
using InnoClinic.Profiles.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountsController : ProfilesController<Account, AccountModel>
{
    public AccountsController(ILogger<AccountsController> logger,
        IAccountService service,
        IMapper mapper) : base(logger, service, mapper) { }

    [HttpGet]
    public async Task<IActionResult> GetAllAccountsAsync()
    {
        return await GetAllAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetAccountByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> AddAccountAsync([FromBody] AccountModel model)
    {
        return await AddAsync(model);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteAccountAsync(Guid id)
    {
        return await DeleteAsync(id);
    }
}