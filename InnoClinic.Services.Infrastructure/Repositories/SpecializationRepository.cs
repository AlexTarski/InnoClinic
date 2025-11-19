using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Infrastructure.Repositories
{
    public class SpecializationRepository : CrudRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(ServicesContext context, ILogger<SpecializationRepository> logger)
            : base(context, logger) { }
    }
}