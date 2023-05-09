using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ftrip.io.framework.HealthCheck
{
    public class HealthCheckUIInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public HealthCheckUIInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            var basePath = Environment.GetEnvironmentVariable("HEALTH_CHECK_BASE_PATH") ?? "";
            _services.AddHealthChecksUI(setup =>
            {
                setup.SetEvaluationTimeInSeconds(60);
                setup.AddHealthCheckEndpoint($"Basic Health Check", $"{basePath}/api/health");
            }).AddInMemoryStorage();
        }
    }
}