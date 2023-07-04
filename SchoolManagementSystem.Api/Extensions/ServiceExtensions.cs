﻿using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.Configurations;
using SchoolManagementSystem.Infrastructure.DataContext;
using SchoolManagementSystem.Infrastructure.Repository;
using SchoolManagementSystem.Service.Implementation;

namespace SchoolManagementSystem.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<ITenantRegistry, TenantRegistry>();
            services.AddScoped<ITenantResolver, TenantResolver>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

            services.AddScoped<ITeachingStaff, TeachingStaff>();
            services.AddScoped<ISalaryService, SalaryService>();
        }
    }
}
