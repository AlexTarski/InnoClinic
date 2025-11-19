using System.Linq;

using InnoClinic.Services.Business.Filters;
using InnoClinic.Services.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Business.Services
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