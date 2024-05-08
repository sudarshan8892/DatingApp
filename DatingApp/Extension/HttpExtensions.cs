using DatingApp.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace DatingApp.Extension
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader paginationHeader)
        {

            var JsonOption = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, JsonOption));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");



        }
    }
}
