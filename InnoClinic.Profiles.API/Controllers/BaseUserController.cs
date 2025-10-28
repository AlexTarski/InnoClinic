using AutoMapper;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.API.Controllers;

public abstract class BaseUserController<T, K> : ControllerBase
    where T : User
    where K : class
{
    private protected readonly ILogger<BaseUserController<T, K>> _logger;
    private readonly IEntityService<T> _service;
    private readonly IMapper _mapper;

    protected BaseUserController(ILogger<BaseUserController<T, K>> logger,
        IEntityService<T> service,
        IMapper mapper)
    {
        _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
        _service = service ?? throw new DiNullReferenceException(nameof(service));
        _mapper = mapper ?? throw new DiNullReferenceException(nameof(mapper));
    }

    protected async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<K>>(result));
    }

    //TODO: review this endpoint
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
    }

    //TODO: review this endpoint
    protected async Task<IActionResult> GetByAccountIdAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    //TODO: review this endpoint
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
    }

    //TODO: review this endpoint
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
    }
}