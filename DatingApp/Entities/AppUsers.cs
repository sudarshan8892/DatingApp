using DatingApp.Entities;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using WebAPIDatingAPP.DATA;
using WebAPIDatingAPP.Extension;

namespace WebAPIDatingAPP.Entities
{
    public class AppUsers
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }   
        public byte[] PasswordSalt { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public List<Photo>Photos{ get; set; }=new();

        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }

    }
}

