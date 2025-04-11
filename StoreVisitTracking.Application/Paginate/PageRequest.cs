using System.ComponentModel.DataAnnotations;

namespace StoreVisitTracking.Application.Paginate
{
    public class PageRequest
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; } = 0;

        [Range(1, MaxPageSize)]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
