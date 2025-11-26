using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chat.Repo.Migrations
{
    /// <inheritdoc />
    public partial class Oldpassedaway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JwtAccessExpiryMinutes",
                table: "Apps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JwtAccessExpiryMinutes",
                table: "Apps");
        }
    }
}
