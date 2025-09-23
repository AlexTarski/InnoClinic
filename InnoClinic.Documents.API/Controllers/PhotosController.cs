using System;
using System.Threading.Tasks;

using InnoClinic.Documents.Business.Interfaces;
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

        //TODO: (1) Handle Errors, (2) Add For other types, (3) Check name generation (do not get file name now)
        [HttpPost("Doctors")]
        public async Task<IActionResult> AddDoctorPhotoAsync(IFormFile formFile)
        {
            return await AddAsync(formFile, Business.UploadFileType.PhotoDoctor);
        }
    }
}