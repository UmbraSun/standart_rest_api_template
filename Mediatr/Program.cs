using Swashbuckle.AspNetCore.SwaggerUI;
using template_asp.net_application.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Mediatr v1");
        c.DocumentTitle = "service API Documentation";

        c.DefaultModelRendering(ModelRendering.Example);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayOperationId();
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CultureMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();
app.AutoMigrateDb();

app.Run();
