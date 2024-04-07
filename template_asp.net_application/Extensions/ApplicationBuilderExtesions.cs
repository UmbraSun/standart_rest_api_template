namespace template_asp.net_application.Extensions
{
    public static class ApplicationBuilderExtesions
    {
        public static IApplicationBuilder BuildForDev(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

        public static IApplicationBuilder BuildApp(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();

            return app;
        }
    }
}
