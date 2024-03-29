﻿using Microsoft.Extensions.Primitives;
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
            var tokenQueryParam = context.Request.Query["token"];

            if (StringValues.IsNullOrEmpty(authHeader) && StringValues.IsNullOrEmpty(tokenQueryParam))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            if(StringValues.IsNullOrEmpty(authHeader))
            {
                context.Request.Headers.Authorization = tokenQueryParam;
            }

            await next.Invoke(context);
        }
    }
}
