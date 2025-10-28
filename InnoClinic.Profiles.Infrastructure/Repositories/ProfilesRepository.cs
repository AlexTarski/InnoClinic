using System.Reflection;

using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Repositories
{
    public class ProfilesRepository : IProfilesRepository
    {
        private readonly static Dictionary<Type, ProfileType> _profilesMap;
        private readonly static MethodInfo _userExistsMethod;
        private readonly ProfilesContext _context;

        static ProfilesRepository()
        {
            var dbSetProperties = GetProfilesContextProperties();
            var profileMap = MapUserToProfileType(dbSetProperties);
            _profilesMap = profileMap;
            _userExistsMethod = GetUserExistsMethod();
        }
        public ProfilesRepository(ProfilesContext context)
        {
            _context = context ??
                throw new DiNullReferenceException(nameof(context));
        }

        public async Task<ProfileType> GetProfileTypeAsync(Guid accountId)
        {
            foreach (var userAndProfileType in _profilesMap)
            {
                var method = _userExistsMethod!.MakeGenericMethod(userAndProfileType.Key);

                var userExists = await (Task<bool>)method.Invoke(this, [accountId]);

                if (userExists)
                {
                    return userAndProfileType.Value;
                }
            }

            return ProfileType.UnknownProfile;
        }

        private async Task<bool> UserExistsAsync<T>(Guid accountId) 
            where T : User
        {
            return await _context.Set<T>().AnyAsync(entity => entity.AccountId == accountId);
        }

        private static List<PropertyInfo> GetProfilesContextProperties()
        {
            var dbSetProperties = typeof(ProfilesContext)
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            return dbSetProperties.ToList();
        }

        private static Dictionary<Type, ProfileType> MapUserToProfileType(List<PropertyInfo> dbSetProperties)
        {
            var profileMap = dbSetProperties
                .Select(prop => new
                {
                    Type = prop.PropertyType.GetGenericArguments()[0],
                    EnumValue = Enum.Parse<ProfileType>(prop.PropertyType.GetGenericArguments()[0].Name)
                })
                .ToDictionary(x => x.Type, x => x.EnumValue);

            return profileMap;
        }

        private static MethodInfo GetUserExistsMethod()
        {
            var repoType = typeof(ProfilesRepository);
            var methodInfo = repoType
                .GetMethod(nameof(UserExistsAsync), BindingFlags.Instance | BindingFlags.NonPublic);

            return methodInfo!;
        }
    }
}