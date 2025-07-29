// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.Diagnostics;
using System.Net;
namespace CleanArchExample.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // gọi middleware tiếp theo
            }
            catch (Exception ex)
            {
                var traceId = Activity.Current?.TraceId.ToString();
                _logger.LogError(ex, "Unhandled Exception - TraceId: {TraceId}", traceId);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var error = new
                {
                    message = ex.Message,
                    statusCode = context.Response.StatusCode,
                    traceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
