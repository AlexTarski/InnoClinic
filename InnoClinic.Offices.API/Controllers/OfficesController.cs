using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Business.Models;
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
                throw new ArgumentNullException(nameof(service), $"{nameof(service)} cannot be null");
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null");
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

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] OfficeModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
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