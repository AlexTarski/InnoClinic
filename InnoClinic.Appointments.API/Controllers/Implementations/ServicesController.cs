using System;
using System.Threading.Tasks;

using AutoMapper;

using InnoClinic.Services.Business.Filters;
using InnoClinic.Services.Business.Interfaces;
using InnoClinic.Services.Business.Models;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.DataSeeding.Entities.ProfileTypes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.API.Controllers.Implementations
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServicesController : BaseEntityCrudController<Service, ServiceParameters, ServiceModel>
    {
        public ServicesController(ILogger<ServicesController> logger, IServiceService service, IMapper mapper)
            : base(logger, service, mapper) { }

        [HttpGet]
        public async Task<IActionResult> GetAllServicesAsync([FromQuery] ServiceParameters serviceParameters)
        {
            if (!IsReceptionist())
            {
                Logger.Information(_logger, $"User not a {nameof(Receptionist)}. " +
                    $"Only {nameof(Receptionist)} allowed to see inactive {nameof(Service)}s." +
                    $"Return only active {nameof(Service)}s.");

                serviceParameters.OnlyActive = true;
            }

            return await GetAllFilteredAsync(serviceParameters);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetServiceByIdAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }
    }
}