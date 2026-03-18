using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbOperationsWithEFCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class addedCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CategoryId",
                table: "Authors",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Categories_CategoryId",
                table: "Authors",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Categories_CategoryId",
                table: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CategoryId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Authors");
        }
    }
}
