using InnoClinic.Profiles.Business.Filters;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.Pagination;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Business.Services;

public class PatientService : UserService<Patient, PatientParameters>, IPatientService
{
    public PatientService(IPatientsRepository repository, ILogger<PatientService> logger)
        : base(repository, logger) {}

    public async override Task<PagedList<Patient>> GetAllFilteredAsync(PatientParameters queryParams)
    {
        var query = _repository.GetEntityQuery();

        return await _repository.GetAllAsync(query, queryParams);
    }
}