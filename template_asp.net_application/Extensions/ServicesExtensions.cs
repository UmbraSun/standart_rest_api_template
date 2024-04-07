using DAL.ApplicationDbContext;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using template_asp.net_application.Infrastructure;

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
            services.AddSwaggerGen();

            services.AddDbContext<AppMsSqlDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("Default")));

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

            return services;
        }
    }
}
