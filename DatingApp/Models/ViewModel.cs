using DatingApp.DTOs;

namespace DatingApp.Models
{
    public class ViewModel
    {
        public List<MemberDTo>Users { get; set; }
        public _UserParams Pagination { get; set; }
        public List<LikedDto> likedDtos{ get; set; }
        public List<MessageDto> Message { get; set; }

        public IEnumerable<MessageDto> MessagesThread { get; set; }
        public PaginationDto Paginations { get; set; }
        //sudarshan
        public string Predicate { get; set; } = "liked";
        public string Container { get; set; } = "Unread";
        public MemberDTo member { get; set; }
        public string userName { get; set; }


    }
}
