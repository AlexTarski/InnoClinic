using System.Linq;

using InnoClinic.Appointments.Business.Filters;
using InnoClinic.Appointments.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared.DataSeeding.Entities.ProfileTypes;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Business.Services
{
    public class SpecializationService : EntityService<Specialization, SpecializationParameters>, ISpecializationService
    {
        public SpecializationService(ISpecializationRepository repository, ILogger<SpecializationService> logger)
            : base(repository, logger) { }

        public override void ApplyFilters(ref IQueryable<Specialization> query, SpecializationParameters queryParams)
        {
            if (queryParams.OnlyActive)
            {
                query = query.Where(spec => spec.IsActive);
            }
        }
    }
}