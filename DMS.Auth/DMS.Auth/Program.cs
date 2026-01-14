using DMS.Auth.Feature.Auth.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("AuthDatabase"));
});

//Service registrations
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddSingleton<JwtTokenService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

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
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
