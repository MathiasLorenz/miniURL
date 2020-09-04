using Microsoft.AspNetCore.Builder;

namespace MiniURL.API.Common
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}