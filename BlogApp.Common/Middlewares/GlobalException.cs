using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Common.Middlewares
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Internal server error occurred. Please try again.";
            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Error";
            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                    title = "Warning";
                message = "Too many requests have been made.";
                statusCode = StatusCodes.Status429TooManyRequests;
                await ModifyHeader(context, title, message, statusCode);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                    title = "Alert";
                message = "Not authorized to access.";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                    title = "Out of reach";
                message = "Not allowed to access.";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is TaskCanceledException)
                {
                    message = "Request timeout";
                    statusCode = StatusCodes.Status408RequestTimeout;
                    title = "Request timeout";
                }
                else
                    await ModifyHeader(context, title, message, statusCode);
            }
        }
        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new ProblemDetails()
                {
                    Detail = message,
                    Status = statusCode,
                    Title = title
                }),
                context.RequestAborted
            );
        }
    }
}
