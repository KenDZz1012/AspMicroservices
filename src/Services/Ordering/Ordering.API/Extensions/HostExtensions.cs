using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host, Action<T,IServiceProvider> seeder,int? retry = 0) where T : DbContext
        {
            int retryForAvailability = retry.Value;
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<T>>();
                var context = services.GetService<T>();
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}");

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}");
                }
                catch(SqlException ex)
                {
                    throw;
                }
            }
            return host;

        }

        private static void InvokeSeeder<T>(Action<T, IServiceProvider> seeder, T context, IServiceProvider services) where T : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
