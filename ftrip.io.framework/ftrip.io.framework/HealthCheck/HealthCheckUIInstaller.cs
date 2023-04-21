using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

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
            _services.AddHealthChecksUI(setup =>
            {
                setup.SetEvaluationTimeInSeconds(60);
                setup.AddHealthCheckEndpoint($"Basic Health Check", "/api/health");
            }).AddInMemoryStorage();
        }
    }
}