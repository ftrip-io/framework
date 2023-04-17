using ftrip.io.framework.ExceptionHandling.Exceptions;
using System.Net;

namespace ftrip.io.framework.ExceptionHandling
{
    public static class HttpCodeMapper
    {
        public static HttpStatusCode From(HandlableException exception)
        {
            return exception switch
            {
                BadLogicException _ => HttpStatusCode.BadRequest,
                AuthorizationException _ => HttpStatusCode.Unauthorized,
                ForbiddenException _ => HttpStatusCode.Forbidden,
                MissingEntityException _ => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}