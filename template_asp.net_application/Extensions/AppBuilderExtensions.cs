using DAL.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace template_asp.net_application.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void AutoMigrateDb(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppMsSqlDbContext>();
            context.Database.Migrate();
        }
    }
}
