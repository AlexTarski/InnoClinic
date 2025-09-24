using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using InnoClinic.Documents.Business;
using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Http;
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
                var result = await _service.GetAllAsync();
                return Ok(result);
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
                Logger.Warning(_logger, ex, $"Failed to get by ID: {id}");
                return NotFound($"{typeof(T).Name} with ID {id} was not found");
            }
        }

        protected async Task<IActionResult> AddAsync(IFormFile file, UploadFileType uploadFileType)
        {
            try
            {
                var photoId = await _service.AddAsync(file, uploadFileType);

                return Ok(photoId);
            }
            catch (InvalidEnumArgumentException ex)
            {
                Logger.Error(_logger, ex, $"Invalid {nameof(UploadFileType)}");
                return BadRequest($"Invalid {nameof(UploadFileType)}");
            }
            catch (UploadFailedException ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                return StatusCode(StatusCodes.Status502BadGateway,
                    "File upload failed due to external server error. Please try again later");
            }
        }
    }
}