namespace DatingApp.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 20;
        public int PageNumber { get; set; } =1;
        private int _PageSize = 5;

      

        public int PageSize
        {
            get =>_PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public string OrderBy { get; set; } = "LastActive";

    }
}
