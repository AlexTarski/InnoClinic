using AutoMapper;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Models;
using InnoClinic.Profiles.Domain.Entities;
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

    [HttpPost]
    public async Task<IActionResult> AddPatientAsync([FromBody] PatientModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newPatient = _mapper.Map<PatientModel, Patient>(model);
            var success = await _service.AddEntityAsync(newPatient);

            return success ? Created($"/{newPatient.Id}", _mapper.Map<Patient, PatientModel>(newPatient))
                                      : StatusCode(500, "Failed to save a new patient");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to save a new patient with ID {Id}", model.Id);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to save a new patient: {Message}", ex.Message);
            return BadRequest($"Patient with ID {model.Id} already exists");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save a new patient with ID {Id}", model.Id);
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }
}