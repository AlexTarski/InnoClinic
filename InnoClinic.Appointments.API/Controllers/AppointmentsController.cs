using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using InnoClinic.Appointments.Business.Filters;
using InnoClinic.Appointments.Business.Interfaces;
using InnoClinic.Appointments.Business.Models;
using InnoClinic.Appointments.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InnoClinic.Appointments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IAppointmentService _service;
        private readonly IMapper _mapper;

        public AppointmentsController(ILogger<AppointmentsController> logger, IAppointmentService service,
            IMapper mapper)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _service = service ?? throw new DiNullReferenceException(nameof(service));
            _mapper = mapper ?? throw new DiNullReferenceException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServicesAsync([FromQuery] AppointmentParameters queryParams)
        {
            var result = await _service.GetAllFilteredAsync(queryParams);
            AddPaginationHeader(result.TotalCount, result.PageSize, result.CurrentPage, result.TotalPages,
                result.HasNext, result.HasPrevious);

            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetServiceByIdAsync(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                return Ok(_mapper.Map<AppointmentModel>(result));
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warning(_logger, ex, $"Failed to get by ID: {id}");

                return NotFound($"{nameof(Appointment)} with ID {id} was not found");
            }
        }
        
        private void AddPaginationHeader(int totalCount, int pageSize, int currentPage, int totalPages,
            bool hasNext, bool hasPrevious)
        {
            var metadata = new
            {
                totalCount,
                pageSize,
                currentPage,
                totalPages,
                hasNext,
                hasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        }
    }
}