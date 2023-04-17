using ftrip.io.framework.ExceptionHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ftrip.io.framework.ExceptionHandling.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HandlableException exception)
            {
                await HandleResponseBasedOnHandbleException(context, exception);
            }
            catch (Exception exception)
            {
                await HandleResponseBasedOnUnexpectedException(context, exception);
            }
        }

        private Task HandleResponseBasedOnHandbleException(HttpContext context, HandlableException exception)
        {
            var httpStatusCode = HttpCodeMapper.From(exception);

            return HandleResponse(context, httpStatusCode, exception.Message, exception.GetType().Name);
        }

        private Task HandleResponseBasedOnUnexpectedException(HttpContext context, Exception exception)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;

            return HandleResponse(context, httpStatusCode, exception.Message, exception.GetType().Name);
        }

        private Task HandleResponse(HttpContext context, HttpStatusCode httpStatusCode, string message, string exceptionType)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            var responseObject = JObject.FromObject(new
            {
                StatusCode = httpStatusCode,
                ErrorMessage = message,
                ExceptionType = exceptionType
            });

            return context.Response.WriteAsync(responseObject.ToString());
        }
    }
}