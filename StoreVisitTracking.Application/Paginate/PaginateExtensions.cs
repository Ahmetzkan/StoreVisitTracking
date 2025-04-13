using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.Messages;

namespace StoreVisitTracking.Application.Paginate
{
    public static class PaginationExtensions
    {
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 0) throw new ArgumentException(ApplicationMessages.PageIndexMustBeGreaterThanOrEqualToZero, nameof(pageIndex));
            if (pageSize <= 0) throw new ArgumentException(ApplicationMessages.PageSizeMustBeGreaterThanZero, nameof(pageSize));

            var totalCount = await source.CountAsync(cancellationToken);

            var items = await source
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new Paginate<T>(items, pageIndex, pageSize, totalCount);
        }

        public static IPaginate<TResult> MapPaginate<TSource, TResult>(
            this IPaginate<TSource> source,
            IMapper mapper)
        {
            var items = mapper.Map<IList<TResult>>(source.Items);

            return new Paginate<TResult>(
                items,
                source.PageIndex,
                source.PageSize,
                source.TotalCount);
        }
    }
}
