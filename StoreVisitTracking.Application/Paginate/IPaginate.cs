namespace StoreVisitTracking.Application.Paginate
{
    public interface IPaginate<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        IList<T> Items { get; }
    }
}
