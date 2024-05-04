using CloudinaryDotNet.Actions;
using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WebAPIDatingAPP.Entities;

namespace DatingApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly HttpClient _client;

        public HomeController(HttpClient client)
        {
            _client = client;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {

            var response = await _client.PostAsJsonAsync("Account/register", register);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
            return RedirectToAction("Error");
        }
        public IActionResult Login() { return View(); }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTo login)
        {

            var response = await _client.PostAsJsonAsync("Account/login", login);

            if (response.IsSuccessStatusCode)
            {
                var loginResult = JsonConvert.DeserializeObject<LoginDTo>(await response.Content.ReadAsStringAsync());

                var sessionData = new LoginDTo
                {
                    Token = loginResult.Token,
                    UserName = loginResult.UserName,
                    PhotoUrl = loginResult?.PhotoUrl
                };

                byte[] sessionDataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sessionData));
                HttpContext.Session.Set("UserSessionData", sessionDataBytes);



                return RedirectToAction("Index", "Matches");
            }
            return RedirectToAction("Error");

        }

        public async Task<IActionResult> ProfileEdit(string username)
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
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(MemberUpdateDTo memberUpdate)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(memberUpdate), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("AppUsers/", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var userName = await response.Content.ReadAsStringAsync();
                return RedirectToAction("ProfileEdit", "Home", new { username = userName });

            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotos(List<IFormFile> files)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);

            var formData = new MultipartFormDataContent();
            files.ForEach(file =>
            {
                formData.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
            });
            var response = await _client.PostAsync("AppUsers/Add-photo", formData);
            await Task.Delay(2000);
            if (response.IsSuccessStatusCode)
            {
                var photos = await response.Content.ReadAsStringAsync();
                var photo = JsonConvert.DeserializeObject<PhotoDTo>(photos);
                return Ok(photo);
            }

            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult>SetMainPhoto(PhotoDTo photoDTo)
        {
            if (!TryGetToken(out string token))
                return View("Error");

            _client.SetBearerToken(token);
            var PhotoId = new StringContent(photoDTo.ToString(), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"AppUsers/set-Main-photo/{photoDTo.Id}", PhotoId);
            if (response.IsSuccessStatusCode)
            {
                LoginDTo update = new LoginDTo
                {
                    PhotoUrl = photoDTo.Url
                };
                byte[] sessionDataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(update));
                HttpContext.Session.Set("UserSessionData", sessionDataBytes);
                return Ok(response);
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Home");
        }
    }
}
