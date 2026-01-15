using DMS.Auth.Feature.JobCard.Endpoint;
using Microsoft.AspNetCore.HttpOverrides;

namespace DMS.Auth.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseAuthApi(this WebApplication app)
        {
            // Apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<AuthDbContext>();

                dbContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DMS Auth API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.MapJobCardEndpoints();
            app.UseGlobalExceptionHandling();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseRateLimiter();
            return app;
        }
    }
}
