using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIDatingAPP.Entities
{
    [Table("Photos")]
    public class Photo
    {

        public int Id { get; set; }
        public string Url { get; set; }
        public bool  IsMain { get; set; }
        public string PublicId { get; set; }
        public int AppuserId { get; set; }
        public AppUsers  AppUsers { get; set; }
    }
}