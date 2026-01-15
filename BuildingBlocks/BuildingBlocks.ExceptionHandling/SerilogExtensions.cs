using Microsoft.Extensions.Configuration;
using Serilog;
namespace BuildingBlocks.ExceptionHandling
{
    public static class SerilogExtensions
    {
        public static LoggerConfiguration ConfigureBaseLogging(
        this LoggerConfiguration logger,
        IConfiguration configuration)
        {
            return logger
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
