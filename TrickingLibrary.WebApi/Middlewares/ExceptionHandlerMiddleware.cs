using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TrickingLibrary.WebApi.Exceptions;

namespace TrickingLibrary.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            // Add your own exception handler here. Use new one catch block.
            catch (TlNotFoundException e)
            {
                await HandleException(httpContext, HttpStatusCode.NotFound, e);
            }
            catch (Exception e)
            {
                await HandleException(httpContext, HttpStatusCode.InternalServerError, e);
            }
        }

        private async Task HandleException(HttpContext httpContext, HttpStatusCode code, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)code;
            await httpContext.Response.WriteAsync(exception.Message);
        }
    }
    
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}