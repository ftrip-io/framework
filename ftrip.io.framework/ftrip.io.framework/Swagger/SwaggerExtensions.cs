using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace ftrip.io.framework.Swagger
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder UseFtripioSwagger(this IApplicationBuilder app, SwaggerUISettings swaggerSettings)
        {
            app.UseSwagger(options => { options.RouteTemplate = swaggerSettings.JsonRoute; });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Description);
                options.SupportedSubmitMethods(new[]
                {
                    SubmitMethod.Get,
                    SubmitMethod.Post,
                    SubmitMethod.Put,
                    SubmitMethod.Delete
                });
            });

            return app;
        }

        public static IApplicationBuilder UseFtripioSwagger(this IApplicationBuilder app, Action<SwaggerUISettings> settingsBuilder)
        {
            var swaggerSettings = new SwaggerUISettings();
            settingsBuilder(swaggerSettings);

            app.UseFtripioSwagger(swaggerSettings);

            return app;
        }
    }
}