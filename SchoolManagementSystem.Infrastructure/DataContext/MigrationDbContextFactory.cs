using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Infrastructure.DataContext
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer();

            return new ApplicationDbContext(builder.Options, new MigrationTenantResolver());
        }

        private class MigrationTenantResolver : ITenantResolver
        {
            private readonly Tenant _tenant;

            public MigrationTenantResolver()
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();

                var defaultConnection = configuration.GetSection("TenantOptions").GetValue<string>("DefaultConnection");

                _tenant = new Tenant { Name = "Migration", ConnectionString = defaultConnection };
            }

            public Tenant GetCurrentTenant() => _tenant;
        }
    }
}
