using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsCommerce.Infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductAttributeValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeMapping_ProductAttribute_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttributeMapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMapping_ProductAttributeId",
                table: "ProductAttributeMapping",
                column: "ProductAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMapping_ProductId",
                table: "ProductAttributeMapping",
                column: "ProductId");
            //Seed the data
            this.SeedData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeMapping");

            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.DropTable(
                name: "Product");
        }

        protected void SeedData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 1, "Brand" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 2, "Model Name" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 3, "Colour" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 4, "Headphones Form Factor" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 5, "Connector Type" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 6, "Size" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 7, "Height" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 8, "Width" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 9, "Length" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 10, "Height" });
            migrationBuilder.InsertData(table: "ProductAttribute", columns: new[] { "Id", "Name" }, values: new object[] { 11, "Item Dimensions LxWxH" });
        }
    }
}
