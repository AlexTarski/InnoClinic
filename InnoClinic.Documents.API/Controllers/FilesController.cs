using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Domain.Entities;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.API.Controllers
{
    public abstract class FilesController<T> : ControllerBase
        where T : File
    {
        protected readonly ILogger<FilesController<T>> _logger;
        protected readonly IFileService<T> _service;

        protected FilesController(ILogger<FilesController<T>> logger, IFileService<T> service)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _service = service ?? throw new DiNullReferenceException(nameof(service));
        }

        protected async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
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
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Failed to get by ID: {Id}", id);
                return NotFound($"{typeof(T).Name} with ID {id} was not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get by ID {Id}", id);
                return StatusCode(500, $"Internal Server Error");
            }
        }
    }
}