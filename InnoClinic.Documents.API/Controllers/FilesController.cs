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
    /// <summary>
    /// An abstract, generic controller that provides basic CRUD operations for file entities.
    /// </summary>
    /// <typeparam name="T">The entity type that derives from <see cref="File"/> and represents a file in the system.</typeparam>
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

        /// <summary>
        /// Retrieves all file entities of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <returns>A list of all entities of type <typeparamref name="T"/>, or an empty list if none are found.</returns>
        protected async Task<IActionResult> GetAllAsync()
        {
                var result = await _service.GetAllAsync();
                return Ok(result);
        }

        /// <summary>
        /// Retrieves a temporary link to a file in storage by its ID.
        /// </summary>
        /// <param name="id">The ID of the file.</param>
        /// <returns>A temporary link to the file in storage.</returns>
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

        /// <summary>
        /// Adds a new entity of type <typeparamref name="T"/> to the database and uploads the associated file to storage.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="uploadFileType">The file type to upload, specified by the corresponding enum value from the controller.</param>
        /// <returns>The ID of the uploaded file.</returns>
        protected async Task<IActionResult> AddAsync(IFormFile file, UploadFileType uploadFileType)
        {
            try
            {
                var fileId = await _service.AddAsync(file, uploadFileType);

                return Ok(fileId);
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

        /// <summary>
        /// Uploads a new file to storage using its ID.
        /// </summary>
        /// <param name="fileId">The ID of the file to upload.</param>
        /// <param name="file">The file to upload.</param>
        /// <returns>NoContent status code if the operation succeeds.</returns>
        protected async Task<IActionResult> UpdateAsync(Guid fileId, IFormFile file)
        {
            try
            {
                await _service.UpdateAsync(fileId, file);
                return NoContent();
            }
            catch (UploadFailedException ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                return StatusCode(StatusCodes.Status502BadGateway,
                    "File upload failed due to external server error. Please try again later");
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Error(_logger, ex, $"Failed to get by ID: {fileId}");
                return NotFound($"{typeof(T).Name} with ID {fileId} was not found");
            }
        }
    }
}