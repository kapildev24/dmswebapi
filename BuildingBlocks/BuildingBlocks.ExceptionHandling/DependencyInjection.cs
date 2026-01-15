using BuildingBlocks.ExceptionHandling.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.ExceptionHandling
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers exception handling services
        /// </summary>
        /// 
        public static IServiceCollection AddGlobalExceptionHandling(
            this IServiceCollection services)
        {
            // No services needed right now,
            // but this allows future extensibility
            return services;
        }

        /// <summary>
        /// Adds global exception handling middleware
        /// </summary>
        /// 
        public static IApplicationBuilder UseGlobalExceptionHandling(
        this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
