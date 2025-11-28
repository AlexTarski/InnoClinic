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
    public class SpecializationsController : BaseEntityCrudController<Specialization, SpecializationParameters, SpecializationModel>
    {
        public SpecializationsController(ILogger<SpecializationsController> logger, ISpecializationService service, IMapper mapper)
            : base(logger, service, mapper) { }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializationsAsync([FromQuery] SpecializationParameters specializationParameters)
        {
            if (!IsReceptionist())
            {
                Logger.Information(_logger, $"User not a {nameof(Receptionist)}. " +
                    $"Only {nameof(Receptionist)} allowed to see inactive {nameof(Specialization)}s." +
                    $"Return only active {nameof(Specialization)}s.");

                specializationParameters.OnlyActive = true;
            }

            return await GetAllFilteredAsync(specializationParameters);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetSpecializationByIdAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }
    }
}