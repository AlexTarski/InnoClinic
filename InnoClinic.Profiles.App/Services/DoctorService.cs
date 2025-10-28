using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Business.Services;

public class DoctorService : UserService<Doctor>, IDoctorService
{
    public DoctorService(IDoctorsRepository doctorsRepository, ILogger<DoctorService> logger)
        : base(doctorsRepository, logger) {}

    public async Task<bool> IsProfileActiveAsync(Guid accountId)
    {
        var doctorsRepository = (IDoctorsRepository)_repository;
        var profileStatus = await doctorsRepository.GetDoctorStatusAsync(accountId);

        if (profileStatus is null)
            throw new KeyNotFoundException($"Doctor with account id {accountId} not found.");
        
        return profileStatus != DoctorStatus.Inactive;
    }

    public async Task<bool> DeactivateProfilesByOfficeIdAsync(Guid officeId)
    {
        var doctorsRepository = (IDoctorsRepository)_repository;

        var profilesToDeactivate = await doctorsRepository.GetAllByOfficeIdAsync(officeId);

        if (!profilesToDeactivate.Any())
        {
            throw new KeyNotFoundException($"No {nameof(Doctor)} profiles were found for this Office ID: {officeId}");
        }

        foreach (var profile in profilesToDeactivate)
        {
            profile.Status = DoctorStatus.Inactive;
            _repository.UpdateEntity(profile);
        }

        return await SaveAllAsync();
    }
}