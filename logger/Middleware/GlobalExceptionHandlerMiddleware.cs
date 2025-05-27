using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace logger.Middleware
{
    public class GlobalExceptionHandlerMiddleware: IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware>logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }
            catch(Exception ex) 
            {
                Console.WriteLine("im runnng");
                var traceId = Guid.NewGuid();
                _logger.LogError(ex, $"Error occure while processing the request, TraceId : {traceId}," +
                    $" Message : {ex.Message}, StackTrace: {ex.StackTrace}");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    traceId,
                    message = "An internal server error occurred.",
                    details = ex.Message // You can hide this in prod
                };

                var json = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
