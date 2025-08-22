using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Domain
{
    public interface IProfilesRepository
    {
        /// <summary>
        /// Gets the profile type for a given account ID.
        /// </summary>
        /// <param name="accountId">The account ID to retrieve the profile type for.</param>
        /// <returns>The profile type associated with the account ID.</returns>
        Task<ProfileType> GetProfileTypeAsync(Guid accountId);
    }
}