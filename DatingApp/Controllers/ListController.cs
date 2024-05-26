using Microsoft.AspNetCore.Mvc;
using DatingApp.Helpers;
using DatingApp.DTOs;
using Newtonsoft.Json;
using DatingApp.Migrations;
using System;
using CloudinaryDotNet.Actions;
using System.Net.Http.Json;

namespace DatingApp.Controllers
{
    public class ListController : BaseController
    {
        private readonly HttpClient _client;

        public  ListController(HttpClient client)
        {
            _client = client;
        }
        public  async Task< IActionResult >Index()
        {
            if (! TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);
            string predicate = "liked";
            var response = await _client.PostAsJsonAsync($"Likes/{predicate}", new { });
            
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var likedUser = JsonConvert.DeserializeObject<List<LikedDto>>(jsonString);
                return View(likedUser);
            }
            return View("Error");
        }
    }
}
