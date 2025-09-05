using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Offices.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly ILogger<OfficesController> _logger;

        public OfficesController(ILogger<OfficesController> logger,
            IOfficeService service)
        {
            _officeService = service ??
                throw new DiNullReferenceException(nameof(_officeService));
            _logger = logger ??
                throw new DiNullReferenceException(nameof(_logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("/secret")]
        [Authorize]
        public IActionResult GetSecret()
        {
            return Ok("This is a secret message only for authorized users.");
        }
    }
}