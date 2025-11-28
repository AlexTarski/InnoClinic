using System.Linq;

using InnoClinic.Appointments.Business.Filters;
using InnoClinic.Appointments.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Business.Services
{
    public class ServiceCategoryService : EntityService<ServiceCategory, ServiceCategoryParameters>, IServiceCategoryService
    {
        public ServiceCategoryService(IServiceCategoryRepository repository, ILogger<ServiceCategoryService> logger)
            : base(repository, logger) { }

        public override void ApplyFilters(ref IQueryable<ServiceCategory> query, ServiceCategoryParameters queryParams)
        {
        }
    }
}