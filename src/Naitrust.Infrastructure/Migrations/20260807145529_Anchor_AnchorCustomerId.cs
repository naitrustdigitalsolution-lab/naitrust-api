using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Anchor_AnchorCustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnchorCustomerId",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnchorCustomerId",
                table: "Users");
        }
    }
}
