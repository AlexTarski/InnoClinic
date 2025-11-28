using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Infrastructure.Repositories
{
    public class ServiceRepository : CrudRepository<Service>, IServiceRepository
    {
        public ServiceRepository(ServicesContext context, ILogger<ServiceRepository> logger)
            : base(context, logger) { }
    }
}