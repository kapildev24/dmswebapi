using DMS.Auth.Feature.Auth.Logout;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

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
                var connectionString = configuration.GetConnectionString("AuthDatabase");
                
                // Detect database provider based on connection string
                if (connectionString!.Contains("Data Source=") && connectionString.EndsWith(".db"))
                {
                    // SQLite for local development
                    options.UseSqlite(connectionString);
                }
                else
                {
                    // PostgreSQL for production
                    options.UseNpgsql(connectionString);
                }
            });

            // Feature handlers
            services.AddScoped<RegisterHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<RefreshTokenHandler>();
            services.AddScoped<LogoutHandler>();

            // Security
            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<JwtTokenService>();

            // Global exception handling
            services.AddGlobalExceptionHandling();

            // Authentication / Authorization
            services.AddJwtAuthentication(configuration);
            services.AddAuthorization();

            //Fluent Validation
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("login-policy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(15);
                    opt.PermitLimit = 5;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });

                // Custom response when limited
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

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
