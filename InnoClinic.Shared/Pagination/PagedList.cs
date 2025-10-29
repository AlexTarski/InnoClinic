namespace InnoClinic.Shared.Pagination
{
    /// <summary>
    /// Provides a strongly-typed, paginated view over a collection of items of type <typeparamref name="T"/>.
    /// Inherits from <see cref="List{T}"/> to expose the items of the current page while also supplying
    /// pagination metadata such as the current page index, total pages, page size, and total item count.
    /// 
    /// This class validates constructor parameters to prevent invalid pagination states
    /// (e.g., negative counts, invalid page numbers, or page numbers exceeding the available range).
    /// Use <see cref="ToPagedList(IQueryable{T}, int, int)"/> to create an instance directly from a queryable source.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the paginated list.</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Vurrent page number (1-based).
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Total number of pages available, calculated from <see cref="TotalCount"/> and <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Number of items contained in each page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Total number of items across all pages.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether there is a page before the <see cref="CurrentPage"/>.
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether there is a page after the <see cref="CurrentPage"/>.
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class with the specified items and pagination metadata.
        /// </summary>
        /// <param name="items">The items for the current page. Cannot be <c>null</c>.</param>
        /// <param name="count">The total number of items across all pages. Must be greater than or equal to 0.</param>
        /// <param name="pageNumber">The current page number (1-based). Must be within the valid page range.</param>
        /// <param name="pageSize">The number of items per page. Must be greater than or equal to 1.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when any pagination parameter is invalid.</exception>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            ValidateConstructorParameters(items, count, pageNumber, pageSize);
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// Creates a <see cref="PagedList{T}"/> from an <see cref="IQueryable{T}"/> source by applying
        /// pagination logic using <c>Skip</c> and <c>Take</c>.
        /// </summary>
        /// <param name="source">The queryable data source. Cannot be <c>null</c>.</param>
        /// <param name="pageNumber">The page number to retrieve (1-based). Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items per page. Must be greater than or equal to 1.</param>
        /// <returns>
        /// A <see cref="PagedList{T}"/> containing the items for the specified page along with pagination metadata.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="pageNumber"/> or <paramref name="pageSize"/> is invalid.</exception>
        /// <exception cref="OverflowException">Thrown when the calculated skip value exceeds <see cref="int.MaxValue"/>.</exception>
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be at least 1.");
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");

            var count = source.Count();

            // Use long to avoid overflow when calculating skip
            long skipCount = ((long)pageNumber - 1) * pageSize;

            if (skipCount > int.MaxValue)
                throw new OverflowException("Skip value exceeds Int32.MaxValue.");

            var items = source.Skip((int)skipCount).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Validates the parameters passed to the <see cref="PagedList{T}"/> constructor,
        /// ensuring that pagination values are within valid ranges and that the items list is not null.
        /// </summary>
        /// <param name="items">The list of items for the current page. Cannot be <c>null</c>.</param>
        /// <param name="count">The total number of items across all pages. Must be greater than or equal to 0.</param>
        /// <param name="pageNumber">The current page number (1-based). Must be within the valid page range.</param>
        /// <param name="pageSize">The number of items per page. Must be greater than or equal to 1.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="count"/> is negative.</description></item>
        ///   <item><description><paramref name="pageSize"/> is less than 1.</description></item>
        ///   <item><description><paramref name="pageNumber"/> is less than 1.</description></item>
        ///   <item><description><paramref name="pageNumber"/> exceeds the total number of pages when items exist.</description></item>
        ///   <item><description><paramref name="pageNumber"/> is greater than 1 when no items exist.</description></item>
        /// </list>
        /// </exception>
        private static void ValidateConstructorParameters(List<T> items, int count, int pageNumber, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(items);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Total count cannot be negative.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be at least 1.");
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");
            if (count == 0 && pageNumber > 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "No items exist, so only page 1 is valid.");

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number exceeds total pages.");
        }
    }
}