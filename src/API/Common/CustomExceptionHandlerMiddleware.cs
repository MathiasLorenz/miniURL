using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MiniURL.Application.Common.Exceptions;

namespace MiniURL.API.Common
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ctx, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext ctx, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // Base error

            switch (exception)
            {
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            var statusCode = (int)code;
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = statusCode;

            var exceptionMessage = exception.Message;

            var error = SerializeError(statusCode, exceptionMessage);
            _logger.LogError($"Exception thrown in pipeline. Error message: { exceptionMessage }");

            return ctx.Response.WriteAsync(error);
        }

        private string SerializeError(int code, string errorMessage)
        {
            return JsonSerializer.Serialize(new
            {
                code = (int)code,
                error = errorMessage
            });
        }
    }
}