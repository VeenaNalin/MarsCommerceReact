using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsCommerce.Infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            this.SeedData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Product");
        }
        protected void SeedData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 1, "Smartphone" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 2, "Laptop" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 3, "Clothing" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 4, "Footwear" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 5, "Electronics" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 6, "Toys" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 7, "Stationery" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 8, "Pet Items" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 9, "Tools" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 10, "Furniture" });
            migrationBuilder.InsertData(table: "Category", columns: new[] { "Id", "Name" }, values: new object[] { 11, "Sports" });
        }
    }
}
