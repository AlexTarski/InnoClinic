using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Shared.Pagination
{
    /// <summary>
    /// Serves as a base class for defining query string parameters used in paginated API requests.
    /// Provides validated properties for <see cref="PageNumber"/> and <see cref="PageSize"/> with
    /// built-in constraints to prevent invalid pagination values.
    /// </summary>
    public abstract class QueryStringParameters
    {
        /// <summary>
        /// The maximum allowed page size. Any value greater than this will be clamped to <c>MaxPageSize</c>.
        /// </summary>
        private const int MaxPageSize = 50;

        private int _pageNumber = 1;

        /// <summary>
        /// Gets or sets the current page number (1-based).
        /// </summary>
        /// <exception cref="PageOutOfRangeException">
        /// Thrown when the assigned value is less than 1.
        /// </exception>
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value < 1)
                    throw new PageOutOfRangeException(nameof(PageNumber), "Page number must be at least 1.");
                _pageNumber = value;
            }
        }

        private int _pageSize = 10;

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <remarks>
        /// Values less than 1 will throw an <see cref="PageOutOfRangeException"/>.
        /// Values greater than <c>MaxPageSize</c> will be clamped to <c>MaxPageSize</c>.
        /// </remarks>
        /// <exception cref="PageOutOfRangeException">
        /// Thrown when the assigned value is less than 1.
        /// </exception>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                    throw new PageOutOfRangeException(nameof(PageSize), "Page size must be at least 1.");
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
    }
}