using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chat.Repo.Migrations
{
    /// <inheritdoc />
    public partial class AddAggregatorcolumntoGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aggregator",
                table: "Groups",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aggregator",
                table: "Groups");
        }
    }
}
