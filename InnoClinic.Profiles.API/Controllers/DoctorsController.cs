using AutoMapper;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Models;
using InnoClinic.Profiles.Domain.Entities;
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

    [HttpPost]
    public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newDoctor = _mapper.Map<DoctorModel, Doctor>(model);
            var success = await _service.AddEntityAsync(newDoctor);

            return success ? Created($"/{newDoctor.Id}", _mapper.Map<Doctor, DoctorModel>(newDoctor))
                                      : StatusCode(500, "Failed to save a new doctor");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to save a new doctor with ID {Id}", model.Id);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to save a new doctor: {Message}", ex.Message);
            return BadRequest($"Doctor with ID {model.Id} already exists");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save a new doctor with ID {Id}", model.Id);
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }
}