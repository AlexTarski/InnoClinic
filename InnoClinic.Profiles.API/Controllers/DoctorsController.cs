using AutoMapper;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;
[ApiController]
[Route("[Controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ILogger<DoctorsController> _logger;
    private readonly IDoctorService _service;
    private readonly IMapper _mapper;

    public DoctorsController(ILogger<DoctorsController> logger,
        IDoctorService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllDoctorsAsync() 
    {
        try
        {
            var result = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<DoctorModel>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all doctors");
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(_mapper.Map<DoctorModel>(result));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to get a doctor by ID: {Id}", id);
            return NotFound($"Doctor with ID {id} was not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get a doctor by ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}