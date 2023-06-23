using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.Configurations;


namespace SchoolManagementSystem.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
