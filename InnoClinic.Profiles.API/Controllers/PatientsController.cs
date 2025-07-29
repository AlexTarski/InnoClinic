using AutoMapper;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class PatientsController : ControllerBase
{
    private readonly ILogger<PatientsController> _logger;
    private readonly IPatientService _service;
    private readonly IMapper _mapper;

    public PatientsController(ILogger<PatientsController> logger,
        IPatientService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPatientsAsync() 
    {
        try
        {
            var result = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<PatientModel>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all patients");
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(_mapper.Map<PatientModel>(result));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to get a patient by ID: {Id}", id);
            return NotFound($"Patient with ID {id} was not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get a patient by ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}