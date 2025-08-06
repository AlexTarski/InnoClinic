using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnoClinic.Authorization.Business.Interfaces;

namespace InnoClinic.Authorization.API.Controllers;

public abstract class BaseController<T, K> : ControllerBase
    where T : class
    where K : class
{
    private readonly ILogger<BaseController<T, K>> _logger;
    private readonly IEntityService<T> _service;
    private readonly IMapper _mapper;

    protected BaseController(ILogger<BaseController<T, K>> logger,
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
            return Ok(_mapper.Map<IEnumerable<K>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get all");
            return StatusCode(500, $"Internal Server Error:{ex.Message}");
        }
    }

    protected async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(_mapper.Map<K>(result));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to get by ID: {Id}", id);
            return NotFound($"{typeof(T).Name} with ID {id} was not found");
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

            return success
                ? StatusCode(201, $"{typeof(T).Name} created successfully")
                : StatusCode(500, $"Failed to save {typeof(T).Name}");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to save");
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to save: {Message}", ex.Message);
            return BadRequest($"{typeof(T).Name} with the same ID already exists");
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
            return success ? NoContent() : StatusCode(500, $"Failed to delete {typeof(T).Name}");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to delete with ID {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete an entity with ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}