using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Newtonsoft.Json;
using NuGet.Common;

namespace DatingApp.Services
{
    public class UserService:IUserService
    {
        private readonly HttpClient _client;
        private IHttpContextAccessor accessor;

        public UserService(HttpClient client)
        {
            _client = client;
             
        }
        public  async Task<MemberDTo> GetUserByUserNameAsync(string userName, string token)
        {
        

            _client.SetBearerToken(token);
            var response = await _client.GetAsync("AppUsers/" + userName);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MemberDTo>(jsonString);
            }

            return null;
        }
    }
}
