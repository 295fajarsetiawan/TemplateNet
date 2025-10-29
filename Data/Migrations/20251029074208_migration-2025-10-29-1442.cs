using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class migration202510291442 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin123");
            migrationBuilder.Sql($@"
             INSERT INTO ""Users"" (""id"", ""username"", ""password"", ""created_at"")
             VALUES 
             ('{Guid.NewGuid()}', 'admin', '{hashedPassword}', NOW())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
