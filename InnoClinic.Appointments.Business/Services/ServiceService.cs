using System.Linq;

using InnoClinic.Appointments.Business.Filters;
using InnoClinic.Appointments.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Business.Services
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