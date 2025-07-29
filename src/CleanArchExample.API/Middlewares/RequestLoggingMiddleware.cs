// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.Diagnostics;
using System.Text;

namespace CleanArchExample.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var activity = Activity.Current;
            var traceId = activity?.TraceId.ToString() ?? "N/A";

            var request = context.Request;
            var requestBody = string.Empty;

            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            _logger.LogInformation("HTTP Request [{Method}] {Path} TraceId: {TraceId} Body: {Body}",
                request.Method, request.Path, traceId, requestBody);

            await _next(context);
        }
    }
}
