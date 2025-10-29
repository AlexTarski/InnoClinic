using AutoMapper;

using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Models.UserModels;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace InnoClinic.Profiles.API.Controllers;

public abstract class BaseUserController<T, TParams, K> : ControllerBase
    where T : User
    where TParams : QueryStringParameters
    where K : UserModel
{
    protected readonly ILogger<BaseUserController<T, TParams, K>> _logger;
    protected readonly IEntityService<T, TParams> _service;
    protected readonly IMapper _mapper;

    protected BaseUserController(ILogger<BaseUserController<T, TParams, K>> logger,
        IEntityService<T, TParams> service,
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

    protected async Task<IActionResult> GetAllFilteredAsync(TParams queryParams)
    {
        var result = await _service.GetAllFilteredAsync(queryParams);
        AddPaginationHeader(result.TotalCount, result.PageSize, result.CurrentPage, result.TotalPages,
            result.HasNext, result.HasPrevious);

        return Ok(result);
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
            Logger.Warning(_logger, ex, $"Failed to get by ID: {id}");

            return NotFound($"{typeof(T).Name} with ID {id} was not found");
        }
    }

    protected async Task<IActionResult> GetByAccountIdAsync(Guid accountId)
    {
        try
        {
            var result = await _service.GetByAccountIdAsync(accountId);

            return Ok(_mapper.Map<K>(result));
        }
        catch (KeyNotFoundException ex)
        {
            Logger.Warning(_logger, ex, $"Failed to get {typeof(T).Name} by ID: {accountId}");

            return NotFound($"{typeof(T).Name} with account ID {accountId} was not found");
        }
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

    private void AddPaginationHeader(int totalCount, int pageSize, int currentPage, int totalPages,
            bool hasNext, bool hasPrevious)
    {
        var metadata = new
        {
            totalCount,
            pageSize,
            currentPage,
            totalPages,
            hasNext,
            hasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
    }
}