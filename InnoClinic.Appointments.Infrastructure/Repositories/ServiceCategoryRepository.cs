using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Infrastructure.Repositories
{
    public class ServiceCategoryRepository : CrudRepository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(ServicesContext context, ILogger<ServiceCategoryRepository> logger)
            : base(context, logger) { }
    }
}