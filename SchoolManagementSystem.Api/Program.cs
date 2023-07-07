using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Infrastructure.Configurations;
using SchoolManagementSystem.Infrastructure.DataContext;
using System.Reflection;
using SchoolManagementSystem.Infrastructure.MappingProfiles;
using NLog;
using Microsoft.OpenApi.Models;
using SchoolManagementSystem.Api.Extensions;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Service.Implementation;

namespace SchoolManagementSystem.Api;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(o =>
        {
            o.UseSqlServer(options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtHelper.GetTokenParameters();
            });

        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolManagementSystem", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                },
            });
        });

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddAutoMapper(Assembly.Load("SchoolManagementSystem.Infrastructure"));

        builder.Services.ConfigureLoggerService();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        DatabaseHelper.EnsureLatestDatabase(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
            
        app.AddGlobalErrorHandler();

        app.Run();
    }
}
