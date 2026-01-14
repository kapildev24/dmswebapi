using DMS.Auth.Infrastructure.Persistence;

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

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
