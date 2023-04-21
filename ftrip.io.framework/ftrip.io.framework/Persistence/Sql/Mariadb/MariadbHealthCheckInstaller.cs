using ftrip.io.framework.Installers;
using ftrip.io.framework.Persistence.Sql.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Persistence.Sql.Mariadb
{
    public class MariadbHealthCheckInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public MariadbHealthCheckInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            var settings = _services.BuildServiceProvider().GetService<SqlDatabaseSettings>();

            _services.AddHealthChecks()
                .AddMySql(settings.GetConnectionString());
        }
    }
}