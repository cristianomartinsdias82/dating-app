using System.Net.Mime;
using System.Text.Json;
using DatingApp.Api.Errors;

namespace DatingApp.Api.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestProcessorWithExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ProcessRequestsWithExceptionHandlingMiddleware>();
    }

    public class ProcessRequestsWithExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProcessRequestsWithExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ProcessRequestsWithExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ProcessRequestsWithExceptionHandlingMiddleware> logger,
            IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);

                var errorCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = errorCode;

                var apiError = 
                    _hostEnvironment.IsDevelopment() ?
                    new ApiException(errorCode, exc.Message, exc.StackTrace) :
                    new ApiException(errorCode, "An error occurred while attempting to process the request.", default);

                await context.Response.WriteAsJsonAsync<ApiException>(
                    apiError,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }
    }
}