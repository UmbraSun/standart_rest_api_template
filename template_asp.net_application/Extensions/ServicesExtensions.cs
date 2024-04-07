using AutoMapper;
using BLL.Infrastracture;
using BLL.Interfaces;
using BLL.Service;
using DAL.ApplicationDbContext;
using DAL.Models;
using DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using template_asp.net_application.Infrastructure;
using template_asp.net_application.Services;

namespace template_asp.net_application.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            // TODO: added NLog file to app
            //services.AddLogging(logging =>
            //{
            //    logging.AddNLog("NLog.config");
            //    logging.SetMinimumLevel(LogLevel.Information);
            //});
            services.AddDbContext<AppMsSqlDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.Configure<Auth.Jwt>(x => configuration.GetSection(nameof(Auth.Jwt)).Bind(x));
            services.AddServicesAndRepositories();
            services.ConfigMapper();
            services.ConfigAuthorization(configuration);
            services.ConfigSwagger();

            return services;
        }

        public static void AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddTransient<AuthService>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ITestRepository, TestRepository>();
        }

        public static void ConfigMapper(this IServiceCollection services)
        {
            services.AddSingleton(_ => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            }).CreateMapper());
        }

        public static void ConfigAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>(x =>
            {
                x.Password.RequiredLength = 6;
                x.Password.RequireUppercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireDigit = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppMsSqlDbContext>()
                .AddApiEndpoints();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = JwtBuilder.Parameters(configuration);
                });
            services.AddAuthorizationBuilder();
        }

        public static void ConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.DescribeAllParametersInCamelCase();
                x.AddSecurityDefinition(JwtBuilder.Bearer(), new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT",
                    Name = JwtBuilder.Authorization(),
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBuilder.Bearer()
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBuilder.Bearer()
                            },
                            Scheme = "oauth2",
                            Name = JwtBuilder.Bearer(),
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                x.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
            });
        }
    }
}
