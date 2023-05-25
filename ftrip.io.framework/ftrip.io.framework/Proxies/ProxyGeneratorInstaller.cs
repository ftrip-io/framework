using Castle.DynamicProxy;
using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Proxies
{
    public class ProxyGeneratorInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public ProxyGeneratorInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddSingleton(new ProxyGenerator());
        }
    }
}