using DatingApp.DTOs;

namespace DatingApp.Models
{
    public class ViewModel
    {
        public List<MemberDTo>Users { get; set; }
        public _UserParams Pagination { get; set; }
        public List<LikedDto> likedDtos{ get; set; }
        public string Predicate { get; set; } = "liked";

    }
}
