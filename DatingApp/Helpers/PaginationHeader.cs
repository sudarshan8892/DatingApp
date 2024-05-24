namespace DatingApp.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemPerPage, int totelItem, int totelPages)
        {
            CurrentPage = currentPage;
            ItemPerPage = itemPerPage;
            TotelItem = totelItem;
            TotelPages = totelPages;
        }

        public int CurrentPage { get; set; }
        public int ItemPerPage { get; set; }
        public int TotelItem { get; set; }
        public int TotelPages { get; set; } 



    }
}
