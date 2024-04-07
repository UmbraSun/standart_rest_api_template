using template_asp.net_application.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.BuildForDev();

app.BuildApp();
app.MapControllers();
app.Run();
