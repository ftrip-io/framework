using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Secrets
{
    public class EnviromentSecretsManagerInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public EnviromentSecretsManagerInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddSingleton<ISecretsManager, EnviromentSecretsManager>();
        }
    }
}