using Microsoft.Extensions.Primitives;
using System.Net;

namespace Vulnerable.Middlewares
{
    public class CustomAuth
    {
        private readonly RequestDelegate next;

        public CustomAuth(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            var bypass = string.IsNullOrEmpty(path) || path == "/" || path.Contains("login") || path.Contains("register");

            if (bypass)
            {
                await next.Invoke(context);
                return;
            }

            var authHeader = context.Request.Headers.Authorization;

            if (StringValues.IsNullOrEmpty(authHeader))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next.Invoke(context);
        }
    }
}
