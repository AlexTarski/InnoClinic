using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Business.Services;

public class PatientService : UserService<Patient>, IPatientService
{
    public PatientService(IPatientsRepository repository, ILogger<PatientService> logger)
        : base(repository, logger) {}
}