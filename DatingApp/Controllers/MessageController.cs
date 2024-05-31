using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http.Json;

namespace DatingApp.Controllers
{
    public class MessageController : BaseController
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        public MessageController(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(string Container = "Unread")
        {
            if (!TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync($"Messages/?Container={Container}");

            if (response.IsSuccessStatusCode)
            {

                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<PaginationDto>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<List<MessageDto>>(jsonString);
                var viewModel = new ViewModel
                {
                    Message = messages,
                    Paginations = pagination
                };
                return View(viewModel);
            }
            return View("Error");
        }

        public async Task<IActionResult> PartialViewMessage([FromBody] MessageParms messageParms)
        {
            if (!TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync($"Messages/?Container={messageParms.Container}&pageNumber={messageParms.PageNumber}");

            if (response.IsSuccessStatusCode)
            {

                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<_UserParams>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<List<MessageDto>>(jsonString);
                var viewModel = new ViewModel
                {
                    Message = messages,
                    Pagination = pagination,
                    Container = messageParms.Container

                };
                return PartialView("PartialViewMessage", viewModel);
            }
            return View("Error");
        }
        public async Task<IActionResult> PartialViewMessageThread(string userName)
        {
            if (!TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync($"Messages/Thread/{userName}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var messages = JsonConvert.DeserializeObject<IEnumerable<MessageDto>>(jsonString);
                var viewModel = new ViewModel
                {
                    MessagesThread = messages,
                    userName = userName
                };
                return PartialView("PartialViewMessageThread", viewModel);
            }
            return View("Error");
        }
        [HttpPost]
        public async Task<ActionResult> CreateMeaasge([FromBody] CreateMessageDto createMessage)
        {
            if (!TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);

            var response = await _client.PostAsJsonAsync("Messages/", createMessage);
            if (response.IsSuccessStatusCode)
            {
                var message = await _client.GetAsync($"Messages/Thread/{createMessage.RecipientUserName}");

                if (message.IsSuccessStatusCode)
                {
                    var jsonString = await message.Content.ReadAsStringAsync();

                    var messages = JsonConvert.DeserializeObject<IEnumerable<MessageDto>>(jsonString);
                    var viewModel = new ViewModel
                    {
                        MessagesThread = messages,
                        userName = createMessage.RecipientUserName
                    };
                    return PartialView("PartialViewMessageThread", viewModel);
                }
            }
            return View("Error");

        }

        public async Task<ActionResult> DeleteMeaasge(int id)
        {
            if (!TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);

           
            var response = await _client.DeleteAsync($"messages/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)response.StatusCode, new { error = "Error deleting message" });
            }

        }
    }
}
