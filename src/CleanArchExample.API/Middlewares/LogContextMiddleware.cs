// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.Diagnostics;

using Serilog.Context;

namespace CleanArchExample.API.Middlewares
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogContextMiddleware(RequestDelegate next, IHttpContextAccessor accessor)
        {
            _next = next;
            _httpContextAccessor = accessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity?.Name
                : "anonymous";

            using (LogContext.PushProperty("TraceId", traceId))
            using (LogContext.PushProperty("UserId", userId ?? "unknown"))
            {
                await _next(context);
            }
        }
    }
}