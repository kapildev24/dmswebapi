using BuildingBlocks.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DMS.Auth.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthApplicationServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(
                    configuration.GetConnectionString("AuthDatabase"));
            });

            // Feature handlers
            services.AddScoped<RegisterHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<RefreshTokenHandler>();

            // Security
            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<JwtTokenService>();

            // Global exception handling
            services.AddGlobalExceptionHandling();

            // Authentication / Authorization
            services.AddJwtAuthentication(configuration);
            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            var jwt = configuration.GetSection("Jwt");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt["SigningKey"]!))
                    };
                });

            return services;
        }
    }
}
