using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using DatingApp.Models;
using DatingApp.Services;
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
        private readonly IMapper _Mapper;
        private readonly IUserService userService;
        

        public HomeController(HttpClient client, IMapper mapper, IUserService  user)
        {
            _client = client;
            _Mapper = mapper;
            userService = user;

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
            if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Login");
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
        [HttpGet]
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
            LoginDTo LoginDto = SessionHelper.GetLoginDtoFromSession(HttpContext);
            if (LoginDto == null) return NotFound("UserSessionData session");
            var appUsers = await userService.GetUserByUserNameAsync(LoginDto.UserName, token);

            return PartialView("_PhotoPartial", appUsers);

          
        }
        [HttpPost]
        public async Task<IActionResult> SetMainPhoto([FromBody] PhotoDTo photoDTo)
        {
            if (!TryGetToken(out string token))
                return View("Error");


            _client.SetBearerToken(token);
            if (photoDTo == null) return BadRequest();
            var Photoid = new StringContent(photoDTo.ToString(), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"AppUsers/set-Main-photo/{photoDTo.Id}", Photoid);
            if (response.IsSuccessStatusCode)
            {
                #region session
                LoginDTo LoginDto = SessionHelper.GetLoginDtoFromSession(HttpContext);
                if (LoginDto == null) return NotFound("UserSessionData session");
                LoginDto.PhotoUrl = photoDTo.Url;
                byte[] updatedSessionDataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(LoginDto));
                HttpContext.Session.Set("UserSessionData", updatedSessionDataBytes);
                #endregion

                var appUsers = await userService.GetUserByUserNameAsync(LoginDto.UserName, token);
                return PartialView("ProfileEdit", appUsers);

              
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] PhotoDTo photoDTo)
        {
            if (!TryGetToken(out string token))
                return View("Error");


            _client.SetBearerToken(token);
            if (photoDTo == null) return BadRequest();
            var Photoid = new StringContent(photoDTo.ToString(), Encoding.UTF8, "application/json");
            var response = await _client.DeleteAsync($"AppUsers/Delete-Photo/{photoDTo.Id}");
            if (response.IsSuccessStatusCode)
            {
                #region session
                LoginDTo LoginDto = SessionHelper.GetLoginDtoFromSession(HttpContext);
                if (LoginDto == null) return NotFound("UserSessionData session");
                #endregion

                var appUsers = await userService.GetUserByUserNameAsync(LoginDto.UserName, token);
                return PartialView("_PhotoPartial", appUsers);


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
