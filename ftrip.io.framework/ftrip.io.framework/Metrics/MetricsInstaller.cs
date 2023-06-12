using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Metrics
{
    public class MetricsInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public MetricsInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddSingleton<VisitorCounter>();
            _services.AddSingleton<DataTrafficCounter>();
        }
    }
}
