using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.DataContext;

namespace SchoolManagementSystem.Infrastructure.Extensions
{
    public class DatabaseHelper
    {
        public static void EnsureLatestDatabase(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            var connections = provider.GetRequiredService<ITenantRegistry>()
                .GetTenants()
                .Select(e => e.ConnectionString)
                .Distinct();

            foreach (var connection in connections)
            {
                var db = new MigrationDbContextFactory().CreateDbContext(Array.Empty<string>());
                db.Database.SetConnectionString(connection);

                if (db.Database.GetPendingMigrations().Any())
                    db.Database.Migrate();
            }
        }
    }
}
