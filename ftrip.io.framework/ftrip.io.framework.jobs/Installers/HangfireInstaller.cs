using ftrip.io.framework.Installers;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.jobs.Installers
{
    public class HangfireInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public HangfireInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddHangfire(config =>
                    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    .UseMemoryStorage());

            _services.AddHangfireServer();
        }
    }
}