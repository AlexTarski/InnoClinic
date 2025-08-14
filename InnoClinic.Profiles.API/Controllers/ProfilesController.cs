using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

public abstract class ProfilesController<T, K> : ControllerBase
    where T : User
    where K : class
{
    private protected readonly ILogger<ProfilesController<T, K>> _logger;
    private readonly IEntityService<T> _service;
    private readonly IMapper _mapper;

    protected ProfilesController(ILogger<ProfilesController<T, K>> logger,
        IEntityService<T> service,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} must not be null");
        _service = service ?? throw new ArgumentNullException(nameof(service),  $"{nameof(service)} must not be null");
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), $"{nameof(mapper)} must not be null");;
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
            _logger.LogWarning(ex, "Failed to get by ID: {Id}", id);
            return NotFound($"{typeof(T).Name} with ID {id} was not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get by ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    protected async Task<IActionResult> CheckUserExistsAsync(Guid accountId)
    {
        try
        {
            if(await _service.EntityExistsAsync(accountId))
                return Ok($"{typeof(T).Name} with this account ID exists");

            return NotFound($"{typeof(T).Name} with this account ID does not exist");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check by account ID {accountId}", accountId);
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

            return success ? StatusCode(201, $"{typeof(T).Name} created successfully")
                                      : StatusCode(500, $"Failed to save {typeof(T).Name}");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Failed to save");
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
            _logger.LogWarning(ex, "Failed to delete with ID {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete an entity with ID {Id}", id);
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}