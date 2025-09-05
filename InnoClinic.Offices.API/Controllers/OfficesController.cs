using AutoMapper;

using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Business.Models;
using InnoClinic.Offices.Domain;
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
        private readonly IMapper _mapper;

        public OfficesController(ILogger<OfficesController> logger,
            IMapper mapper,
            IOfficeService service)
        {
            _logger = logger ??
                throw new DiNullReferenceException(nameof(_logger));
            _mapper = mapper ??
                throw new DiNullReferenceException(nameof(_mapper));
            _officeService = service ??
                throw new DiNullReferenceException(nameof(_officeService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _officeService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<OfficeModel>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _officeService.GetByIdAsync(id);
                return Ok(_mapper.Map<OfficeModel>(result));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Failed to get by ID: {Id}", id);
                return NotFound($"{typeof(Office).Name} with ID {id} was not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get by ID {Id}", id);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        [HttpGet("/secret")]
        [Authorize]
        public IActionResult GetSecret()
        {
            return Ok("This is a secret message only for authorized users.");
        }
    }
}