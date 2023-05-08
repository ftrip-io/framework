using EmailService.Extensions;
using ftrip.io.framework.email.Settings;
using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.email.Installers
{
    public class EmailDispatcherInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly SMTPSettings _settings;

        public EmailDispatcherInstaller(IServiceCollection services, SMTPSettings settings = null)
        {
            _services = services;
            if (settings == null)
            {
                settings = new FromEnvSMTPSettings();
            }
            _settings = settings;
        }

        public void Install()
        {
            _services.AddEmailDispatcher(options =>
            {
                options.AddServer(_settings.Host, _settings.Port)
                       .AddCredentials(_settings.Username, _settings.Password)
                       .EnableSsl();
            });

            _services.AddTemplateProvider(options =>
            {
                options.SetAssemblyType(typeof(T))
                       .AddFileWithTemplates("templates.json");
            });
        }
    }
}