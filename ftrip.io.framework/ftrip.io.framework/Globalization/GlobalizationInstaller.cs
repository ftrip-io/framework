using ftrip.io.framework.Contexts;
using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;
using System.Resources;

namespace ftrip.io.framework.Globalization
{
    public class GlobalizationInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;

        public GlobalizationInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            var baseName = string.Format($"{typeof(T).Namespace}.Resources.Strings");
            var resourceManager = new ResourceManager(baseName, typeof(T).Assembly);

            _services.AddSingleton(resourceManager);
            _services.AddScoped<GlobalizationContext>();
            _services.AddScoped<IStringManager, StringManager>();

            _services.AddControllers(c => c.Filters.Add<FillGlobalizationContextFilter>());
        }
    }
}