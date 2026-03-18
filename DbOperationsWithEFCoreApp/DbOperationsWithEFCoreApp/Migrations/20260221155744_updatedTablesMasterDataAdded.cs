using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DbOperationsWithEFCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class updatedTablesMasterDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BookPrices");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BookPrices");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "BookPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "BookPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Pakistani Currency", "PKR" },
                    { 2, "American Currency", "USD" },
                    { 4, "Europien Currency", "EUR" },
                    { 5, "Britsh Currency", "GBP" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Pakistani Language", "Urdu" },
                    { 2, "American Language", "English" },
                    { 4, "German Language", "German" },
                    { 5, "Britsh Language", "GBP" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookPrices_BookId",
                table: "BookPrices",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookPrices_CurrencyId",
                table: "BookPrices",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrices_Books_BookId",
                table: "BookPrices",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrices_Currencies_CurrencyId",
                table: "BookPrices",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrices_Books_BookId",
                table: "BookPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPrices_Currencies_CurrencyId",
                table: "BookPrices");

            migrationBuilder.DropIndex(
                name: "IX_BookPrices_BookId",
                table: "BookPrices");

            migrationBuilder.DropIndex(
                name: "IX_BookPrices_CurrencyId",
                table: "BookPrices");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BookPrices");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookPrices");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "BookPrices");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BookPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BookPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
