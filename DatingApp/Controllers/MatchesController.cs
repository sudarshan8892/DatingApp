using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Text;

namespace DatingApp.Controllers
{
    public class MatchesController : BaseController
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public MatchesController(HttpClient client, IMapper  mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index( int pageNumber)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var response = await _client.GetAsync($"AppUsers?pageNumber={pageNumber}");
            if (response.IsSuccessStatusCode)
            {

                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<Pagination>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                
                var appUsers = JsonConvert.DeserializeObject<List<MemberDTo>>(jsonString);

                foreach (var user in appUsers)
                {
                    user.Pagination = pagination;
                }


                return View(appUsers);

            }
            return View("Error");

        }
        public async Task<IActionResult> UserDetails(string username)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var response = await _client.GetAsync("AppUsers/" + username);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var appUsers = JsonConvert.DeserializeObject<MemberDTo>(jsonString);
                return View(appUsers);
            }
            

            return View("Error");

        }
    }

}
