using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly ILogger<PhotosController> _logger;

        public PhotosController(ILogger<PhotosController> logger)
        {
            _logger = logger;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }
    }
}