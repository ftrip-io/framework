using System;

namespace ftrip.io.framework.Swagger
{
    public class FromEnvSwaggerSettings : SwaggerSettings
    {
        public override string ApiPathPrefix { get => Environment.GetEnvironmentVariable("API_PREFIX") ?? ""; }
    }
}