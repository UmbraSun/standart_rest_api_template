using Swashbuckle.AspNetCore.SwaggerUI;
using template_asp.net_application.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.DefaultModelExpandDepth(3);
        x.DefaultModelRendering(ModelRendering.Example);
        x.DefaultModelsExpandDepth(-1);
        x.DisplayOperationId();
        x.DisplayRequestDuration();
        x.DocExpansion(DocExpansion.None);
        x.EnableDeepLinking();
        x.EnableFilter();
        x.ShowExtensions();
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
