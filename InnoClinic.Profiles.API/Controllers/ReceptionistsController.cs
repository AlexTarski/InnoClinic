using AutoMapper;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Models;
using InnoClinic.Profiles.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class ReceptionistsController : ControllerBase
{
    private readonly ILogger<ReceptionistsController> _logger;
    private readonly IReceptionistService _service;
    private readonly IMapper _mapper;

    public ReceptionistsController(ILogger<ReceptionistsController> logger,
        IReceptionistService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllReceptionistsAsync() 
    {
        try
        {
            var result = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ReceptionistModel>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all receptionists");
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(_mapper.Map<ReceptionistModel>(result));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to get a receptionist by ID: {Id}", id);
            return NotFound($"Receptionist with ID {id} was not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get a receptionist by ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddReceptionistAsync([FromBody] ReceptionistModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newReceptionist = _mapper.Map<ReceptionistModel, Receptionist>(model);
            var success = await _service.AddEntityAsync(newReceptionist);

            return success ? Created($"/{newReceptionist.Id}", _mapper.Map<Receptionist, ReceptionistModel>(newReceptionist))
                                      : StatusCode(500, "Failed to save a new receptionis");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to save a new receptionist with ID {Id}", model.Id);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to save a new receptionist: {Message}", ex.Message);
            return BadRequest($"Receptionist with ID {model.Id} already exists");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save a new receptionist with ID {Id}", model.Id);
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }
}