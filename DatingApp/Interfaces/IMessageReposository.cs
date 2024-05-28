using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Interfaces
{
    public interface IMessageReposository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task <Message> GetMessage (int Id);   
        Task <PageList<MessageDto>>GetMessageForUser( MessageParms messageParms);   
        Task <IEnumerable<MessageDto>>GetMessageThread(string CurrentUserName,  string RecipientUserName);
        Task<bool> SaveAllAsync(); 
    }
}
