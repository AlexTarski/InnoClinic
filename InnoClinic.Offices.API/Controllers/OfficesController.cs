using AutoMapper;

using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Business.Models;
using InnoClinic.Offices.Domain;
using InnoClinic.Shared;
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

        /// <summary>
        /// Get all offices with pagination support. To get all offices without pagination, just leave parameters empty.
        /// </summary>
        /// <param name="page">Represents page</param>
        /// <param name="pagesize">Count of elements on the page</param>
        /// <returns></returns>
        [HttpGet("{page?}/{pagesize?}")]
        public async Task<IActionResult> GetAllAsync(int page = 0, int pagesize = 0)
        {
            try
            {
                var result = await _officeService.GetAllAsync(page, pagesize);
                return Ok(_mapper.Map<IEnumerable<OfficeModel>>(result));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logger.Error(_logger, ex, $"Failed to get all {nameof(Office)}s with pagination");
                return BadRequest("Invalid page or page size parameter");
            }
            catch (Exception ex)
            {
                Logger.Error(_logger, ex, $"Failed to get all {nameof(Office)}s with pagination: internal server error.");
                return StatusCode(500, $"Internal server error. Please try again later.");
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
                Logger.Warning(_logger, ex, $"Failed to get {nameof(Office)} by ID: {id}");
                return NotFound($"{nameof(Office)} with ID {id} was not found");
            }
            catch (Exception ex)
            {
                Logger.Error(_logger, ex, $"Failed to get {nameof(Office)} by ID {id}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //TODO: Remove this endpoint after testing
        [Authorize(Roles = UserRoles.Receptionist)]
        [HttpGet("/secret")]
        public IActionResult GetSecret()
        {
            return Ok("This is a secret message only for authorized users.");
        }

        //TODO: Remove this endpoint after testing
        [HttpGet("/debug-claims")]
        public IActionResult DebugClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}