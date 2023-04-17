using ftrip.io.framework.Installers;
using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Repository;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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
            IMongoClient mongoClient = new MongoClient(_settings.GetConnectionString());

            var mongoDatabase = mongoClient.GetDatabase(_settings.Database);

            _services.AddSingleton(mongoClient);

            _services.AddSingleton(mongoDatabase);

            _services.AddScoped<IUnitOfWork, UnitOfWork>();

            _services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }
    }
}