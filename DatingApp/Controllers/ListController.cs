using Microsoft.AspNetCore.Mvc;
using DatingApp.Helpers;
using DatingApp.DTOs;
using Newtonsoft.Json;
using DatingApp.Migrations;
using System;
using CloudinaryDotNet.Actions;
using System.Net.Http.Json;
using DatingApp.Models;
using WebAPIDatingAPP.Entities;

namespace DatingApp.Controllers
{
    public class ListController : BaseController
    {
        private readonly HttpClient _client;

        public  ListController(HttpClient client)
        {
            _client = client;
        }
        public  async Task< IActionResult >Index( string predicate= "liked")
        {
            if (! TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);
          
            var response = await _client.PostAsJsonAsync($"Likes/?predicate={predicate}", new { });
            
            if (response.IsSuccessStatusCode)
            {

                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<_UserParams>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                var likedUser = JsonConvert.DeserializeObject<List<LikedDto>>(jsonString);
                var viewModel = new ViewModel
                {
                    likedDtos = likedUser,
                    Pagination = pagination
                };
                return View(viewModel);
            }
            return View("Error");
        }
        public async Task<IActionResult> partialViewLike([FromBody] LikesParams userParams)
        {
            if (!TryGetToken(out string token))
                return View("Error");
            _client.SetBearerToken(token);

            var response = await _client.PostAsJsonAsync($"Likes/?predicate={userParams.Predicate}&pageNumber={userParams.PageNumber}", new { });

            if (response.IsSuccessStatusCode)
            {
                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<_UserParams>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                var likedUser = JsonConvert.DeserializeObject<List<LikedDto>>(jsonString);
                
                var viewModel = new ViewModel
                {
                    likedDtos = likedUser,
                    Pagination = pagination,
                    Predicate= userParams.Predicate
                };
                return PartialView("partialViewLike", viewModel);
               
            }
            return Json(new { success = false, error = "An error occurred" });
        }
    }
}
