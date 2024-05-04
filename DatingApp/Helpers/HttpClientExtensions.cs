using System.Net.Http.Headers;

namespace DatingApp.Helpers
{
    public static class HttpClientExtensions
    {
        public static void SetBearerToken(this HttpClient httpClient, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
