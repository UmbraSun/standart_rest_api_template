using AutoMapper;
using BLL.Infrastracture;
using BLL.Interfaces;
using BLL.Service;
using BLL.Services;
using DAL.ApplicationDbContext;
using DAL.Models;
using DTOs;
using Mediatr.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repositories;
using System.Reflection;
using template_asp.net_application.Services;

namespace Mediatr.Extensions
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
            services.AddSwaggerDependency(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddSwaggerDependency(this IServiceCollection services, Assembly executingAssembly)
        {
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Backup service api",
                        Version = "v1"
                    });

                    c.MapType<FileResult>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    });

                    c.DescribeAllParametersInCamelCase();

                    c.SupportNonNullableReferenceTypes();

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Description = "Please enter JWT with Bearer into field",
                        Name = "Authorization",
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                    });

                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{executingAssembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath, true);

                    // Set the comments for referenced assemblies (Projects)
                    var referencedAssembliesNames = executingAssembly.GetReferencedAssemblies().Distinct();
                    foreach (var assemblyName in referencedAssembliesNames)
                    {
                        var relatedXmlFile = $"{assemblyName.Name}.xml";
                        var relatedXmlPath = Path.Combine(AppContext.BaseDirectory, relatedXmlFile);
                        if (File.Exists(relatedXmlPath))
                        {
                            c.IncludeXmlComments(relatedXmlPath, true);
                        }
                    }

                    c.UseAllOfForInheritance();
                    c.UseOneOfForPolymorphism();

                    c.SelectSubTypesUsing(baseType =>
                        baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType))
                    );
                    c.CustomSchemaIds(x =>
                    {
                        if (x.FullName == null)
                        {
                            return x.DeclaringType!.Name!.Replace("`", "") + "." + x.Name;
                        }
                        if (x.FullName.Contains("`"))
                        {
                            return x.Name.Substring(0, x.Name.IndexOf('`')) + string.Join("", x.GenericTypeArguments.Select(x => x.Name));
                        }

                        return x.Name;
                    });
                });

                return services;
            }
        }

        public static void AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddTransient<AuthService>();
            services.AddTransient<TestRepository>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<PartnersRepository>();
            services.AddTransient<IPartnersService, PartnersService>();
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
    }
}
