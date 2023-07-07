using SchoolManagementSystem.Infrastructure.Configurations;

namespace SchoolManagementSystem.Api.Extensions;

public static class ApplicationBuilderExtention
{
    public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
}
