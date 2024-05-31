using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DatingApp.Controllers
{
    public class MatchesController : BaseController
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public MatchesController(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        #region Matches
        public async Task<IActionResult> Index()
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var response = await _client.GetAsync("AppUsers");
            if (response.IsSuccessStatusCode)
            {
                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<_UserParams>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                var appUsers = JsonConvert.DeserializeObject<List<MemberDTo>>(jsonString);

                var viewModel = new ViewModel
                {
                    Users = appUsers,
                    Pagination = pagination
                };
                return View(viewModel);
            }
            return View("Error");
        }


        public async Task<IActionResult> PartialView([FromBody] _UserParams userParams)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var response = await _client.GetAsync($"AppUsers?pageNumber={userParams.pageNumber}&MinAge={userParams.MinAge}&MaxAge={userParams.MaxAge}&Gender={userParams.Gender}&OrderBy={userParams.OrderBy}");
            if (response.IsSuccessStatusCode)
            {
                var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<_UserParams>(paginationHeader);

                var jsonString = await response.Content.ReadAsStringAsync();
                var appUsers = JsonConvert.DeserializeObject<List<MemberDTo>>(jsonString);

                var viewModel = new ViewModel
                {
                    Users = appUsers,
                    Pagination = pagination
                };
                return PartialView("PartialView", viewModel);
            }
            return View("Error");
        }

        #endregion
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
                var viewModel = new ViewModel
                {
                    member=appUsers
                };
                return View(viewModel);
            }


            return View("Error");

        }
        #region likes
        public async Task<ActionResult> AddLike(string UserName)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var response = await _client.PostAsJsonAsync($"Likes/{UserName}", new { });

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, username = UserName });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, error = responseContent });
            }
        }
        #endregion
    }

}
