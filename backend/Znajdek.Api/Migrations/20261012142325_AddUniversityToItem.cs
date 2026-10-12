using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Znajdek.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversityToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "University",
                table: "Items");
        }
    }
}
