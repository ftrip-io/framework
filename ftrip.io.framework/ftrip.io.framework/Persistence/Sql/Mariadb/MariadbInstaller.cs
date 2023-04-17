using ftrip.io.framework.Installers;
using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.Sql.Repository;
using ftrip.io.framework.Persistence.Sql.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Persistence.Sql.Mariadb
{
    public class MariadbInstaller<T> : IInstaller where T : DbContext
    {
        private readonly IServiceCollection _services;
        private readonly SqlDatabaseSettings _settings;

        public MariadbInstaller(IServiceCollection services, SqlDatabaseSettings settings = null)
        {
            _services = services;
            if (settings == null)
            {
                settings = new FromEnvSqlDatabaseSettings();
            }

            _settings = settings;
        }

        public void Install()
        {
            _services.AddScoped<IUnitOfWork, UnitOfWork>();

            _services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            _services.AddScoped(typeof(DbContext), typeof(T));

            var mainAssemblyName = typeof(T).Assembly.GetName().Name;
            _services.AddDbContext<T>(options =>
                       options.UseMySql(_settings.GetConnectionString(),
                            builder =>
                            {
                                builder.MigrationsAssembly(mainAssemblyName);
                                builder.UseNewtonsoftJson();
                            }));

            _services.AddSingleton(_settings);
        }
    }
}