using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace ftrip.io.framework.Persistence.Sql.Migrations
{
    public static class MigrateDbContextExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
                var retry = Policy.Handle<Exception>().WaitAndRetryForever(attempt =>
                {
                    var waitFor = new[]
                    {
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromSeconds(15)
                    };
                    return waitFor[attempt % 3];
                });

                retry.Execute(() =>
                {
                    context.Database.Migrate();
                });
                logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
            }

            return host;
        }
    }
}