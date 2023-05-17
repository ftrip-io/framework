using ftrip.io.framework.ExceptionHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Serilog;
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
            var name = exception.GetType().Name;
            var message = exception.Message;

            var logger = GetLogger(context);
            logger.Error("Handable exception has occured - Exception[{Exception}], Message[{Message}]", name, message);

            return HandleResponse(context, httpStatusCode, name, message);
        }

        private Task HandleResponseBasedOnUnexpectedException(HttpContext context, Exception exception)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;
            var name = exception.GetType().Name;
            var message = exception.Message;

            var logger = GetLogger(context);
            logger.Error("Unexpected exception has occured - Exception[{Exception}], Message[{Message}]", name, message);

            return HandleResponse(context, httpStatusCode, message, name);
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

        private ILogger GetLogger(HttpContext context)
        {
            return GetService<ILogger>(context);
        }

        private T GetService<T>(HttpContext context)
        {
            return (T)context.RequestServices.GetService(typeof(T));
        }
    }
}