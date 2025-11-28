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
    public class ServiceCategoriesController : BaseEntityCrudController<ServiceCategory, ServiceCategoryParameters, ServiceCategoryModel>
    {
        public ServiceCategoriesController(ILogger<ServiceCategoriesController> logger, IServiceCategoryService service, IMapper mapper)
            : base(logger, service, mapper) { }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceCategoriesAsync([FromQuery] ServiceCategoryParameters serviceCategoryParameters)
        {
            if (!IsReceptionist())
            {
                Logger.Information(_logger, $"User not a {nameof(Receptionist)}. " +
                    $"Only {nameof(Receptionist)} allowed to see inactive {nameof(ServiceCategory)} list." +
                    $"Return only active {nameof(ServiceCategory)} list.");

                //serviceParameters.OnlyActive = true;
            }

            return await GetAllFilteredAsync(serviceCategoryParameters);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetServiceCategoryByIdAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }
    }
}