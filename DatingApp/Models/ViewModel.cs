using DatingApp.DTOs;

namespace DatingApp.Models
{
    public class ViewModel
    {
        public List<MemberDTo>Users { get; set; }
        public _UserParams Pagination { get; set; }
    }
}
