using BuildingBlocks.ExceptionHandling;
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

builder.Services.AddAuthApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseAuthApi();

app.Run();