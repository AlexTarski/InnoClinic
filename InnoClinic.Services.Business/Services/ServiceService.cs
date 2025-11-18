using System.Linq;

using InnoClinic.Services.Business.Filters;
using InnoClinic.Services.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Business.Services
{
    public class ServiceService : EntityService<Service, ServiceParameters>, IServiceService
    {
        public ServiceService(IServiceRepository repository, ILogger<ServiceService> logger)
            : base(repository, logger) { }

        public override void ApplyFilters(ref IQueryable<Service> query, ServiceParameters queryParams)
        {
            if (queryParams.OnlyActive)
            {
                query = query.Where(service => service.IsActive);
            }
        }
    }
}