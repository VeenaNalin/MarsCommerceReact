using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsCommerce.Infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddingPropeerties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StockKeepingUnit",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "StockKeepingUnit",
                table: "Product");
        }
    }
}
