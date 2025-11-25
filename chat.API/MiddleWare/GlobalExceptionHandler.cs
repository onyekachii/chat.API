using chat.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace chat.API.MiddleWare
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string message;
            switch (ex)
            {
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "Chat API: Access denied";
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "Chat API: Resource not found";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Chat API: Internal Server Error";
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiErrorResponse
            (
                StatusCode : context.Response.StatusCode,
                Message : message
            );

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}

public record ApiErrorResponse(int StatusCode, string Message);