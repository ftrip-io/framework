using ftrip.io.framework.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ftrip.io.framework.auth
{
    public class FillCurrentUserContextFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var logger = GetService<ILogger>(httpContext);
                var currentUserContext = GetService<CurrentUserContext>(httpContext);

                currentUserContext.Id = GetClaimValue(httpContext, ClaimTypes.Name);
                currentUserContext.Role = GetClaimValue(httpContext, ClaimTypes.Role);

                logger.Information("Extracted data from HttpContext - UserId[{UserId}], Role[{Role}]", currentUserContext.Id, currentUserContext.Role);
            }

            await next();
        }

        private T GetService<T>(HttpContext context)
        {
            return (T)context.RequestServices.GetService(typeof(T));
        }

        private string GetClaimValue(HttpContext httpContext, string claimType) =>
            httpContext.User.Claims.Where(claim => claim.Type == claimType).Select(claim => claim.Value).FirstOrDefault();
    }
}