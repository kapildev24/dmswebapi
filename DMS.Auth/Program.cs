using DMS.Auth.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ConfigureBaseLogging(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console()
        .WriteTo.File(
            "logs/log-.txt",
            rollingInterval: RollingInterval.Day);
});

//Service registrations
builder.Services.AddAuthApplicationServices(builder.Configuration);

var app = builder.Build();

//Middleware pipeline
app.UseAuthApi();

app.Run();
