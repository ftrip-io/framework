using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Correlation
{
    public class CorrelationInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public CorrelationInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddScoped(typeof(CorrelationContext));
        }
    }
}