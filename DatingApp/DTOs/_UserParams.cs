using DatingApp.Helpers;
using Newtonsoft.Json;

namespace DatingApp.DTOs
{
    public class _UserParams:PaginationDto
    {

        public int pageNumber { get; set; } = 1;

        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;

        public string OrderBy { get; set; } = "LastActive";


    }
}
