using InnoClinic.Shared.Pagination;

namespace InnoClinic.Profiles.Business.Filters
{
    /// <summary>
    /// Serves for defining query string parameters used in paginated and filtered API requests.
    /// Provides validated properties for <see cref="PageNumber"/> and <see cref="PageSize"/> with
    /// built-in constraints to prevent invalid pagination values.
    /// <see cref="PageNumber"/> limits maximum allowed page size. Any value greater than this will be clamped to <c>MaxPageSize</c>.
    /// </summary>
    public class DoctorParameters : QueryStringParameters
    {
        public bool OnlyActiveProfiles { get; set; } = true;
    }
}