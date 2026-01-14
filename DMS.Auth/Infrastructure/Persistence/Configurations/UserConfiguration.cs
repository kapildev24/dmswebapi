namespace DMS.Auth.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(x => x.Email)
                .IsUnique(); // 🔥 Critical for concurrency safety

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();
        }
    }
}
