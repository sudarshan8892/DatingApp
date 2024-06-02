using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPIDatingAPP.Entities;

namespace WebAPIDatingAPP.DATA
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUsers> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;
            var users = await File.ReadAllTextAsync("DATA/UserSeedData.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new DateOnlyConverter());
            var appusers = JsonSerializer.Deserialize<List<AppUsers>>(users, options);
            foreach (var user in appusers)
            {
                

                user.UserName = user.UserName.ToLower();


                await userManager.CreateAsync(user, "Shetty8892&&");
            }
           

        }
    }

    partial class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}
