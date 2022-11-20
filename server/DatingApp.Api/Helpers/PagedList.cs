using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int PageSize { get; } = 10;
        public int PageNumber { get; } = 1;
        public int ItemCount { get; }

        protected PagedList(
            IEnumerable<T> items,
            int itemCount,
            int pageSize,
            int pageNumber)
        {
            AddRange(items);
            PageNumber = pageNumber;
            PageSize = pageSize;
            ItemCount = itemCount;
        }

        public int PageCount => (int)Math.Ceiling(ItemCount / (double)PageSize);

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source,
            PaginationParameters paginationParameters,
            CancellationToken cancellationToken = default)
            => await CreateAsync(
                source,
                paginationParameters.PageNumber,
                paginationParameters.PageSize,
                cancellationToken);

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
            {
                if (source is null)
                    throw new ArgumentException(nameof(source));

                var itemCount = await source.CountAsync(cancellationToken);
                var items = await source
                                    .Skip(((pageNumber <= 0 ? 1 : pageNumber) - 1) * (pageSize <= 0 ? 10 : pageSize))
                                    .Take(pageSize <= 0 ? 10 : pageSize)
                                    .ToListAsync(cancellationToken);

                return new PagedList<T>(items,
                                        itemCount,
                                        pageSize,
                                        pageNumber);
            }
    }
}