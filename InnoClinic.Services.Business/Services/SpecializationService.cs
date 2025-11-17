using System.Linq;

using InnoClinic.Services.Business.Filters;
using InnoClinic.Services.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Business.Services
{
    public class SpecializationService : EntityService<Specialization, SpecializationParameters>, ISpecializationService
    {
        public SpecializationService(ISpecializationRepository repository, ILogger<SpecializationService> logger)
            : base(repository, logger) { }

        public override void ApplyFilters(ref IQueryable<Specialization> query, SpecializationParameters queryParams)
        {
        }
    }
}