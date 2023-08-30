using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsCommerce.Infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class MigrationForPaymentInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInfo", x => x.Id);
                });
            this.SeedData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentInfo");
        }
        protected void SeedData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "PaymentInfo", columns: new[] { "Id", "PaymentMethod" }, values: new object[] { 1, "Cash On Delivery" });
            migrationBuilder.InsertData(table: "PaymentInfo", columns: new[] { "Id", "PaymentMethod" }, values: new object[] { 2, "Credit Card" });
            migrationBuilder.InsertData(table: "PaymentInfo", columns: new[] { "Id", "PaymentMethod" }, values: new object[] { 3, "Debit Card" });

        }
    }
}
