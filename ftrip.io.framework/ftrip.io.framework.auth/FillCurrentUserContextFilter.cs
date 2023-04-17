using ftrip.io.framework.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
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
            var currentUserContext = GetService<CurrentUserContext>(httpContext);

            currentUserContext.Id = GetClaimValue(httpContext, ClaimTypes.Name);
            currentUserContext.Role = GetClaimValue(httpContext, ClaimTypes.Name);

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