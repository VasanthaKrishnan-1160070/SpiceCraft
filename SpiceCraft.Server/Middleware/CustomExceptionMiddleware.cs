using Newtonsoft.Json;
using System.Net;

namespace SpiceCraft.Server.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = isDevelopment ? exception.Message : "An unexpected error occurred.",
                Detailed = isDevelopment ? exception.StackTrace : null
            };

            var result = JsonConvert.SerializeObject(errorResponse);
            return context.Response.WriteAsync(result);
        }
    }

}
