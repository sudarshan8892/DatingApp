using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebAPIDatingAPP.DATA;

namespace DatingApp.DATA
{
    public class MessageRepository : IMessageReposository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int Id)
        {
            return await _context.Messages.FindAsync(Id);
        }

        public async Task<PageList<MessageDto>> GetMessageForUser(MessageParms messageParms)
        {
            var query = _context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

            query = messageParms.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUserName == messageParms.UserName),
                "Outbox" => query.Where(u => u.SenderUserName == messageParms.UserName),
                _ => query.Where(u => u.RecipientUserName == messageParms.UserName && u.DateRead == null)

            };
            var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PageList<MessageDto>.CreateAsync(message, messageParms.PageNumber, messageParms.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string CurrentUserName, string RecipientUserName)
        {
            var meaasge = await _context.Messages
                .Include(u => u.Sender)
                .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                .ThenInclude(p => p.Photos)
                .Where(
                m => m.RecipientUserName == CurrentUserName.ToLower() &&
                m.SenderUserName == RecipientUserName.ToLower() ||
                m.RecipientUserName == RecipientUserName.ToLower() &&
                m.SenderUserName == CurrentUserName.ToLower()

                ).OrderByDescending(m => m.MessageSent)
                .ToListAsync();


            #region example
            /* Let's use an example with actual usernames to make it clearer:

             CurrentUserName: Alice
             RecipientUserName: Bob
             The query will select messages where:

            Alice received a message from Bob:
                      m.RecipientUserName == "Alice" && m.SenderUserName == "Bob"
            Bob received a message from Alice:
                     m.RecipientUserName == "Bob" && m.SenderUserName == "Alice"
            
             */
            #endregion
            var UnreadMessage = meaasge.Where(u => u.DateRead == null &&
              u.RecipientUserName == CurrentUserName).ToList();



            if (UnreadMessage.Any())
            {
                foreach (var messages in UnreadMessage)
                {
                    messages.DateRead = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }




            return _mapper.Map<IEnumerable<MessageDto>>(meaasge);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
