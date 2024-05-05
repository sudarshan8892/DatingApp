using DatingApp.DTOs;
using DatingApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DatingApp.Controllers
{
    public class MatchesController : BaseController
    {
        private readonly HttpClient _client;

        public MatchesController(HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var response = await _client.GetAsync("AppUsers");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                
                var appUsers = JsonConvert.DeserializeObject<List<MemberDTo>>(jsonString);

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
