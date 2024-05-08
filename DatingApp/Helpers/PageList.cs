using Microsoft.EntityFrameworkCore;

namespace DatingApp.Helpers
{
    public class PageList<T> : List<T>
    {
        public PageList(IEnumerable<T> Item, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(Item);
        }

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }



        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count= await source.CountAsync();
            var Item = await source.Skip((pageNumber - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

            return new PageList<T>(Item, count, pageNumber, pageSize);
        }
    }
}
