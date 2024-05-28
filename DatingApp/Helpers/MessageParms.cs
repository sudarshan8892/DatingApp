namespace DatingApp.Helpers
{
    public class MessageParms:PaginationParams
    {
        public string UserName { get; set; }
        public string Container { get; set; } = "Unread";
 
    }
}
