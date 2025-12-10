using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chat.Repo.Migrations
{
    /// <inheritdoc />
    public partial class DisplayNamecolumninMessagetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Messages");
        }
    }
}
