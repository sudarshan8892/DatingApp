using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAPIDatingAPP.DATA;
using WebAPIDatingAPP.Entities;

namespace DatingApp.DTOs
{
    public class MemberDTo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string  PhotoUrl { get; set; }
        public int Age  { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } 
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<PhotoDTo> Photos { get; set; }

        public Pagination Pagination { get; set; }

    }

    public class Pagination
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
