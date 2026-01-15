using DMS.Auth.Feature.JobCard.Entity;

namespace DMS.Auth.Infrastructure.Persistence
{
    public sealed class AuthDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<JobCard> JobCards => Set<JobCard>();

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AuthDbContext).Assembly);
        }
    }
}
