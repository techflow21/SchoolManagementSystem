using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Infrastructure.Configurations;
using SchoolManagementSystem.Infrastructure.DataContext;
using SchoolManagementSystem.Infrastructure.Extensions;
using System.Reflection;
using SchoolManagementSystem.Infrastructure.MappingProfiles;
using NLog;

namespace SchoolManagementSystem.Api
{
    public class Program
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

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddAutoMapper(Assembly.Load("SchoolManagementSystem.Infrastructure"));

            builder.Services.ConfigureLoggerService();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            app.Run();
        }
    }
}