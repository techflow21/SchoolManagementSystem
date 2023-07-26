using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.Configurations;
using SchoolManagementSystem.Infrastructure.DataContext;
using SchoolManagementSystem.Infrastructure.Repository;
using SchoolManagementSystem.Service.ExternalServices;
using SchoolManagementSystem.Service.Implementation;

namespace SchoolManagementSystem.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLoggerService(this IServiceCollection services)
    {

        services.AddSingleton<ILoggerManager, LoggerManager>();
        services.AddSingleton<ITenantRegistry, TenantRegistry>();
        services.AddScoped<ITenantResolver, TenantResolver>();
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IContactService, ContactService>();

        services.AddScoped<ITeachingStaff, TeachingStaff>();
        services.AddScoped<ISalaryService, SalaryService>();

        services.AddScoped<IManageIncome, ManageIncome>();
        services.AddScoped<IManageExpenditure, ManageExpenditure>();

        services.AddScoped<ISchoolFeeService, SchoolFeeService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IPhotoUploadService, PhotoUploadService>();

        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<ISubjectService, SubjectService>();

    }
}
