using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
		public int PageSize { get; set; }
        public int RangeWidth { get; set; } = 3;

		public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageSize = PageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public int ActiveRangeFrom
        {
            get
            {
                var from = PageIndex - Math.Floor(RangeWidth / 2.0);
                if (from > 1)
                {
                    var to = PageIndex + Math.Floor(RangeWidth / 2.0);
                    if (to > TotalPages)
                    {
                        from -= to - TotalPages;
                    }
                }
                return Math.Max(1, (int)from);
            }
            
        }

        public int ActiveRangeTo
        {
            get
            {
                return Math.Max(1, Math.Min(ActiveRangeFrom + RangeWidth - 1, TotalPages));
            }
            
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
