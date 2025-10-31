using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Profiles.Business.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly IProfilesRepository _repository;

        public ProfilesService(IProfilesRepository repository)
        {
            _repository = repository ?? throw new DiNullReferenceException(nameof(repository));
        }
        public Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            return _repository.GetProfileTypeAsync(accountId);
        }
    }
}