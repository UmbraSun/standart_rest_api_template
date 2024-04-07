using Resources;

namespace template_asp.net_application.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddResources();

            // Add a resource manager for multilingualism
            services.AddResources();



            return services;
        }

        public static IServiceCollection AddResources(this IServiceCollection services) => services.AddScoped<IResourceManager, ResourceManager>();
    }
}
