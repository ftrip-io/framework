using ftrip.io.framework.Installers;
using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Repository;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using MongoDatabaseSettings = ftrip.io.framework.Persistence.NoSql.Mongodb.Settings.MongoDatabaseSettings;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Installers
{
    public class MongodbInstaller : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly MongoDatabaseSettings _settings;

        public MongodbInstaller(IServiceCollection services, MongoDatabaseSettings settings = null)
        {
            _services = services;
            if (settings == null)
            {
                settings = new FromEnvMongoDatabaseSettings();
            }

            _settings = settings;
        }

        public void Install()
        {
            var clientSettings = MongoClientSettings.FromConnectionString(_settings.GetConnectionString());
            clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
            IMongoClient mongoClient = new MongoClient(clientSettings);

            var mongoDatabase = mongoClient.GetDatabase(_settings.Database);

            _services.AddSingleton(_settings);

            _services.AddSingleton(mongoClient);

            _services.AddSingleton(mongoDatabase);

            _services.AddScoped<IUnitOfWork, UnitOfWork>();

            _services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }
    }
}