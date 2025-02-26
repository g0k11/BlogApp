using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.AuthenticationApi.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    context.Items["UserId"] = userId;
                }

                if (!string.IsNullOrEmpty(userRole))
                {
                    context.Items["UserRole"] = userRole;
                }
            }

            await _next(context);
        }
    }
}
