using ftrip.io.framework.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ftrip.io.framework.Globalization
{
    public class FillGlobalizationContextFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var globalizationContext = GetContext<GlobalizationContext>(httpContext);

            globalizationContext.PreferedLanguage = GetPreferedLanguage(httpContext);

            await next();
        }

        private T GetContext<T>(HttpContext context)
        {
            return (T)context.RequestServices.GetService(typeof(T));
        }

        private string GetPreferedLanguage(HttpContext httpContext)
        {
            var acceptedLanguages = httpContext.Request.Headers["Accept-Language"].ToString().Trim();
            if (string.IsNullOrEmpty(acceptedLanguages) || acceptedLanguages == "*")
            {
                return "jp";
            }

            return acceptedLanguages.Split(",")[0];
        }
    }
}