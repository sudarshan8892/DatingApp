using AutoMapper;
using DatingApp.Apiconfig;
using DatingApp.Controllers;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPIDatingAPP.DATA;
using WebAPIDatingAPP.Helpers;
using WebAPIDatingAPP.Interfaces;
using WebAPIDatingAPP.Middleware;
using WebAPIDatingAPP.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
builder.Services.AddHttpClient();

builder.Services.AddHttpClient<IUserService, UserService>();

builder.Services.AddScoped<ITokenService, TokenServices>();
builder.Services.AddScoped<IRepository, UserRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<LogUserActivity>();

builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("cloudinaryConnection"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSession();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokenkey"])),
        ValidateIssuer = false,
        ValidateAudience = false,

    };

});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:1000"));
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
try
{
    var context = service.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = service.GetService<ILogger<Program>>();
    logger.LogError(ex.Message, "error accured during Migrations");
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
