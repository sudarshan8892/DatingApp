using System.Net;
using System.Text.Json;
using WebAPIDatingAPP.Error;

namespace WebAPIDatingAPP.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var respose = _environment.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                     : new ApiException(context.Response.StatusCode, ex.Message, "Internal Serevr error");

                var option= new JsonSerializerOptions { PropertyNamingPolicy=JsonNamingPolicy.CamelCase}; 
                var json=JsonSerializer.Serialize(respose, option);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
