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
                // Step 1: Drop the foreign key constraint first
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    DROP CONSTRAINT ""FK_RefreshTokens_Users_UserId"";
                ");

                // Step 2: Alter Users table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""Users"" 
                    ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid,
                    ALTER COLUMN ""CreatedAtUtc"" TYPE timestamp without time zone USING ""CreatedAtUtc""::timestamp without time zone;
                ");

                // Step 3: Alter RefreshTokens table (including UserId)
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid,
                    ALTER COLUMN ""UserId"" TYPE uuid USING ""UserId""::uuid,
                    ALTER COLUMN ""ExpiresAtUtc"" TYPE timestamp without time zone USING ""ExpiresAtUtc""::timestamp without time zone,
                    ALTER COLUMN ""RevokedAtUtc"" TYPE timestamp without time zone USING ""RevokedAtUtc""::timestamp without time zone;
                ");

                // Step 4: Re-add the foreign key constraint
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    ADD CONSTRAINT ""FK_RefreshTokens_Users_UserId"" 
                    FOREIGN KEY (""UserId"") 
                    REFERENCES ""Users"" (""Id"") 
                    ON DELETE CASCADE;
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
                // Step 1: Drop the foreign key constraint
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    DROP CONSTRAINT ""FK_RefreshTokens_Users_UserId"";
                ");

                // Step 2: Revert Users table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""Users"" 
                    ALTER COLUMN ""Id"" TYPE text,
                    ALTER COLUMN ""CreatedAtUtc"" TYPE text;
                ");

                // Step 3: Revert RefreshTokens table
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    ALTER COLUMN ""Id"" TYPE text,
                    ALTER COLUMN ""UserId"" TYPE text,
                    ALTER COLUMN ""ExpiresAtUtc"" TYPE text,
                    ALTER COLUMN ""RevokedAtUtc"" TYPE text;
                ");

                // Step 4: Re-add the foreign key constraint
                migrationBuilder.Sql(@"
                    ALTER TABLE ""RefreshTokens"" 
                    ADD CONSTRAINT ""FK_RefreshTokens_Users_UserId"" 
                    FOREIGN KEY (""UserId"") 
                    REFERENCES ""Users"" (""Id"") 
                    ON DELETE CASCADE;
                ");
            }
        }
    }
}
