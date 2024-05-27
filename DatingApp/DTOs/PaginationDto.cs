using Newtonsoft.Json;

namespace DatingApp.DTOs
{
    public class PaginationDto
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("itemPerPage")]
        public int ItemsPerPage { get; set; }

        [JsonProperty("totelItem")]
        public int TotalItems { get; set; }

        [JsonProperty("totelPages")]
        public int TotalPages { get; set; }
    }
}
