using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

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
        public async Task<IActionResult> Index( string Container="Unread")
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

            var response = await _client.GetAsync($"Messages/?Container={messageParms.Container}");

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
                return PartialView("PartialViewMessage", viewModel);
            }
            return View("Error");
        }
    }
}
