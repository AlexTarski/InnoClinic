using AutoMapper;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Models;
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
}