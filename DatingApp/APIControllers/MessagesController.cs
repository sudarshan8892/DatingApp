using AutoMapper;
using DatingApp.Controllers;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extension;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPIDatingAPP.Extension;
using WebAPIDatingAPP.Interfaces;

namespace DatingApp.APIControllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMessageReposository _message;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public MessagesController(IMessageReposository message, IRepository repository, IMapper mapper)
        {
            _message = message;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMeaasge(CreateMessageDto createMessage)
        {
            var userName = User.GetUserName();
            if (userName == createMessage.RecipientUserName.ToLower())
                return BadRequest("you can not send meaasge to yourself");

            var sender = await _repository.GetUserByNameAsync(userName);
            var receipient = await _repository.GetUserByNameAsync(createMessage.RecipientUserName.ToLower());

            if (receipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = receipient,
                SenderUserName = sender.UserName,
                RecipientUserName = receipient.UserName,
                Content = createMessage.Content

            };
            _message.AddMessage(message);
            if (await _message.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
            return BadRequest("Failed to send Meaasge");
        }

        [HttpGet]
        public async Task<ActionResult<PageList<MessageDto>>> GetMessageForUser([FromQuery] MessageParms messageParms)
        {
            messageParms.UserName = User.GetUserName();

            var Message = await _message.GetMessageForUser(messageParms);
            Response.AddPaginationHeader(new PaginationHeader(Message.CurrentPage, Message.PageSize, Message.TotalCount, Message.TotalPage));
            return Message;
        }

        [HttpGet("Thread/{UserName}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>>GetMessageThread( string UserName)
        {
            var CurrentUserName = User.GetUserName();
            return Ok( await _message.GetMessageThread(CurrentUserName, UserName));
        }
    }
}

