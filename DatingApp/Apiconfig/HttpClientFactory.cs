
namespace DatingApp.Apiconfig
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IConfiguration _configuration;

        public HttpClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient CreateClient(string name)
        {
            var baseUrl = _configuration["AppSettings:ApiBaseUrl"];

            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            return client;
        }
    }
}

