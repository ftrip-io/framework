using ftrip.io.framework.Installers;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Installers
{
    public class MongodbHealthCheckInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public MongodbHealthCheckInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            var settings = _services.BuildServiceProvider().GetService<MongoDatabaseSettings>();

            _services.AddHealthChecks()
                .AddMongoDb(settings.GetConnectionString());
        }
    }
}