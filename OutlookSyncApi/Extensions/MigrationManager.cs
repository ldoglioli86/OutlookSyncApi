using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OutlookSyncApi.Migrations;

namespace OutlookSyncApi.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    databaseService.CreateDatabase("OutlookSyncApiDb");

                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                }
                catch
                {
                    throw;
                }
            }

            return host;
        }
    }
}
