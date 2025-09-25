using System;
using System.Threading.Tasks;

using InnoClinic.Documents.Business;
using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Business.Validators;
using InnoClinic.Documents.Domain.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.API.Controllers
{
    /// <summary>
    /// Handles operations related to managing <see cref="Photo"/> in the system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : FilesController<Photo>
    {
        public PhotosController(ILogger<PhotosController> logger, IPhotoService service)
            : base(logger, service) { }

        /// <summary>
        /// Get all <see cref="Photo"/>.
        /// </summary>
        /// <returns>List of <see cref="Photo"/> or empty list</returns>
        /// <response code="200">Returns all existing <see cref="Photo"/></response>
        /// <response code="500">For all internal server errors</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPhotosAsync()
        {
            return await GetAllAsync();
        }

        /// <summary>
        /// Get a temporary link to the <see cref="Photo"/> in a storage by ID.
        /// </summary>
        /// <param name="id"><see cref="Photo"/> ID.</param>
        /// <returns>Temporary link to the <see cref="Photo"/> in a storage by ID.</returns>
        /// <response code="200">Returns temporary link to the <see cref="Photo"/> in a storage by ID.</response>
        /// <response code="404">If a <see cref="Photo"/> with such ID was not found.</response>
        /// <response code="500">For all other internal server errors.</response>
        [HttpGet("{id:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPhotoByIdAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new doctor`s <see cref="Photo"/> in the system and uploads the file that represents the <see cref="Photo"/> to storage.
        /// </summary>
        /// <param name="formFile">The file that represents the <see cref="Photo"/> to add.</param>
        /// <returns>The unique <see cref="Guid"/> ID of the uploaded <see cref="Photo"/>.</returns>
        /// <response code="200">Returns the <see cref="Guid"/> ID of the created <see cref="Photo"/>.</response>
        /// <response code="400">If <paramref name="formFile"/> is invalid, no file was uploaded,
        /// or the uploaded file has an unsupported format (.jpeg, .jpg, .png, .gif).</response>
        /// <response code="500">For all unhandled internal server errors.</response>
        /// <response code="502">If an error occurs during upload to storage.</response>
        [HttpPost("Doctors")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddDoctorPhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoDoctor);
        }

        /// <summary>
        /// Creates a new receptionist`s <see cref="Photo"/> in the system and uploads the file that represents the <see cref="Photo"/> to storage.
        /// </summary>
        /// <param name="formFile">The file that represents the <see cref="Photo"/> to add.</param>
        /// <returns>The unique <see cref="Guid"/> ID of the uploaded <see cref="Photo"/>.</returns>
        /// <response code="200">Returns the <see cref="Guid"/> ID of the created <see cref="Photo"/>.</response>
        /// <response code="400">If <paramref name="formFile"/> is invalid, no file was uploaded,
        /// or the uploaded file has an unsupported format (.jpeg, .jpg, .png, .gif).</response>
        /// <response code="500">For all unhandled internal server errors.</response>
        /// <response code="502">If an error occurs during upload to storage.</response>
        [HttpPost("Receptionists")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddReceptionistPhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoReceptionist);
        }

        /// <summary>
        /// Creates a new patient`s <see cref="Photo"/> in the system and uploads the file that represents the <see cref="Photo"/> to storage.
        /// </summary>
        /// <param name="formFile">The file that represents the <see cref="Photo"/> to add.</param>
        /// <returns>The unique <see cref="Guid"/> ID of the uploaded <see cref="Photo"/>.</returns>
        /// <response code="200">Returns the <see cref="Guid"/> ID of the created <see cref="Photo"/>.</response>
        /// <response code="400">If <paramref name="formFile"/> is invalid, no file was uploaded,
        /// or the uploaded file has an unsupported format (.jpeg, .jpg, .png, .gif).</response>
        /// <response code="500">For all unhandled internal server errors.</response>
        /// <response code="502">If an error occurs during upload to storage.</response>
        [HttpPost("Patients")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddPatientPhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoPatient);
        }

        /// <summary>
        /// Creates a new office`s <see cref="Photo"/> in the system and uploads the file that represents the <see cref="Photo"/> to storage.
        /// </summary>
        /// <param name="formFile">The file that represents the <see cref="Photo"/> to add.</param>
        /// <returns>The unique <see cref="Guid"/> ID of the uploaded <see cref="Photo"/>.</returns>
        /// <response code="200">Returns the <see cref="Guid"/> ID of the created <see cref="Photo"/>.</response>
        /// <response code="400">If <paramref name="formFile"/> is invalid, no file was uploaded,
        /// or the uploaded file has an unsupported format (.jpeg, .jpg, .png, .gif).</response>
        /// <response code="500">For all unhandled internal server errors.</response>
        /// <response code="502">If an error occurs during upload to storage.</response>
        [HttpPost("Offices")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddOfficePhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoOffice);
        }

        /// <summary>
        /// Updates existing file that represents the <see cref="Photo"/> in storage.
        /// </summary>
        /// <param name="fileId">The unique <see cref="Guid"/> ID of the uploading <see cref="Photo"/>.</param>
        /// <param name="formFile">The file that represents the <see cref="Photo"/> to update.</param>
        /// <returns>NoContent status code in case of success.</returns>
        /// <response code="204">Returns only NoContent status code.</response>
        /// <response code="400">If <paramref name="formFile"/> is invalid, no file was uploaded,
        /// or the uploaded file has an unsupported format (.jpeg, .jpg, .png, .gif).</response>
        /// <response code="500">For all unhandled internal server errors.</response>
        /// <response code="502">If an error occurs during upload to storage.</response>
        [HttpPut("{fileId:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> UpdatePhotoAsync(Guid fileId, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return BadRequest("No file uploaded");

            if (!PhotoTypeValidator.IsValidExtension(formFile))
                return BadRequest("File type not allowed. Allowed: .png, .jpg, .jpeg, .gif");

            if (!await PhotoTypeValidator.IsValidContentTypeAsync(formFile))
                return BadRequest($"Invalid content type");

            return await UpdateAsync(fileId, formFile);
        }

        /// <summary>
        /// Provides a common method for adding a new <see cref="Photo"/> to the system.
        /// This method MUST be used by all endpoints that create a new <see cref="Photo"/> 
        /// and upload the corresponding file to storage.
        /// </summary>
        /// <param name="formFile">The file that represents the <see cref="Photo"/> to upload.</param>
        /// <param name="uploadFileType">
        /// The enum value specifying the type of <see cref="Photo"/> being uploaded 
        /// (Doctor, Patient, Office, etc.).
        /// </param>
        /// <returns>The <see cref="Guid"/> identifier of the created <see cref="Photo"/>.</returns>
        private async Task<IActionResult> AddPhotoAsync(IFormFile formFile, UploadFileType uploadFileType)
        {
            if (formFile == null || formFile.Length == 0)
                return BadRequest("No file uploaded");

            if (!PhotoTypeValidator.IsValidExtension(formFile))
                return BadRequest("File type not allowed. Allowed: .png, .jpg, .jpeg, .gif");

            if (!await PhotoTypeValidator.IsValidContentTypeAsync(formFile))
                return BadRequest($"Invalid content type");

            return await AddAsync(formFile, uploadFileType);
        }
    }
}