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
    /// Handles operations related to managing photos in the system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : FilesController<Photo>
    {
        public PhotosController(ILogger<PhotosController> logger, IPhotoService service)
            : base(logger, service) { }

        [HttpGet]
        public async Task<IActionResult> GetAllPhotosAsync()
        {
            return await GetAllAsync();
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetPhotoByIdAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }

        [HttpPost("Doctors")]
        public async Task<IActionResult> AddDoctorPhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoDoctor);
        }

        [HttpPost("Receptionists")]
        public async Task<IActionResult> AddReceptionistPhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoReceptionist);
        }

        [HttpPost("Patients")]
        public async Task<IActionResult> AddPatientPhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoPatient);
        }

        [HttpPost("Offices")]
        public async Task<IActionResult> AddOfficePhotoAsync(IFormFile formFile)
        {
            return await AddPhotoAsync(formFile, UploadFileType.PhotoOffice);
        }

        [HttpPut("{fileId:Guid}")]
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