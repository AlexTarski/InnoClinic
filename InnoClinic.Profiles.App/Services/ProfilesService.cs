using InnoClinic.Shared;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Business.Interfaces;

namespace InnoClinic.Profiles.Business.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly IProfilesRepository _repository;

        public ProfilesService(IProfilesRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository), $"{nameof(repository)} cannot be null");
        }
        public Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            return _repository.GetProfileTypeAsync(accountId);
        }
    }
}