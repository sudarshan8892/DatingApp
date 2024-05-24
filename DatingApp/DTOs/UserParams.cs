using Newtonsoft.Json;

namespace DatingApp.DTOs
{
    public class _UserParams
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("itemPerPage")]
        public int ItemsPerPage { get; set; }

        [JsonProperty("totelItem")]
        public int TotalItems { get; set; }

        [JsonProperty("totelPages")]
        public int TotalPages { get; set; }
        public int pageNumber { get; set; }

        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;

        public string OrderBy { get; set; } = "LastActive";
    }
}
