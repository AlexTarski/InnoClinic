using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models;
using InnoClinic.Profiles.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;
[ApiController]
[Route("api/[Controller]")]
public class DoctorsController : ProfilesController<Doctor, DoctorModel>
{
    
    public DoctorsController(ILogger<DoctorsController> logger,
        IDoctorService service,
        IMapper mapper) : base(logger, service, mapper){ }
    
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
}