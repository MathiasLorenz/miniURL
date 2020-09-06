using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiniURL.Application.Common.Exceptions;

namespace MiniURL.API.Common
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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
            var errorMessage = String.Empty; // Set in switch if the exception does not have a proper message.

            switch (exception)
            {
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)code;

            var error = SerializeError((int)code, errorMessage, exception.Message);

            return ctx.Response.WriteAsync(error);
        }

        private string SerializeError(int code, string? errorMessage, string exceptionMessage)
        {
            return JsonSerializer.Serialize(new
            {
                code = (int)code,
                error = String.IsNullOrEmpty(errorMessage) ? exceptionMessage : errorMessage
            });
        }
    }
}