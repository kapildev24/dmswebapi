using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.Auth.Migrations
{
    /// <inheritdoc />
    public partial class FixPostgreSQLColumnTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only execute for PostgreSQL
            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                // Alter Users table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""Users"" 
                    ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid,
                    ALTER COLUMN ""CreatedAtUtc"" TYPE timestamp without time zone USING ""CreatedAtUtc""::timestamp without time zone;
                ");

                // Alter RefreshTokens table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid,
                    ALTER COLUMN ""UserId"" TYPE uuid USING ""UserId""::uuid,
                    ALTER COLUMN ""ExpiresAtUtc"" TYPE timestamp without time zone USING ""ExpiresAtUtc""::timestamp without time zone,
                    ALTER COLUMN ""RevokedAtUtc"" TYPE timestamp without time zone USING ""RevokedAtUtc""::timestamp without time zone;
                ");
            }
            // For SQLite, no changes needed as it uses TEXT for everything
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Only execute for PostgreSQL
            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                // Revert Users table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""Users"" 
                    ALTER COLUMN ""Id"" TYPE text,
                    ALTER COLUMN ""CreatedAtUtc"" TYPE text;
                ");

                // Revert RefreshTokens table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    ALTER COLUMN ""Id"" TYPE text,
                    ALTER COLUMN ""UserId"" TYPE text,
                    ALTER COLUMN ""ExpiresAtUtc"" TYPE text,
                    ALTER COLUMN ""RevokedAtUtc"" TYPE text;
                ");
            }
        }
    }
}
