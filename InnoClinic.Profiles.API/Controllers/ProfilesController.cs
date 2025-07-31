using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models;
using InnoClinic.Profiles.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

public abstract class ProfilesController<T,K> : ControllerBase
    where T : class
    where K : class
{
    private readonly ILogger<ProfilesController<T,K>> _logger;
    private readonly IEntityService<T> _service;
    private readonly IMapper _mapper;
    
    protected ProfilesController(ILogger<ProfilesController<T,K>> logger,
        IEntityService<T> service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    protected async Task<IActionResult> GetAllAsync() 
    {
        try
        {
            var result = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<T>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all");
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }
    
    protected async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(_mapper.Map<T>(result));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to get by ID: {Id}", id);
            return NotFound($"ID {id} was not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get by ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    protected async Task<IActionResult> AddAsync([FromBody] K model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newEntity = _mapper.Map<K, T>(model);
            var success = await _service.AddEntityAsync(newEntity);

            return success ? StatusCode(201, "Created successfully")
                                      : StatusCode(500, "Failed to save");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to save");
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to save: {Message}", ex.Message);
            return BadRequest($"Entity with the same ID already exists");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save");
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }
    
    protected async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            var success = await _service.DeleteEntityAsync(id);
            return success ? NoContent() : StatusCode(500, "Failed to delete");
        }
        catch(KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to delete an entity with ID {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete an entity with ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}