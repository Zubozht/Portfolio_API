using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class StoreStart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "6eda5005-b3a0-4890-9ba2-449fda36e05c");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "d355978f-dfe1-4bca-a17b-b19529afbd02");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "aspnetroles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31c6dd65-9f8a-4fe3-bc98-d62f2328cb32", null, "Admin", "ADMIN" },
                    { "85040cd6-12ca-47f2-bb6b-811fb2b7779e", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "31c6dd65-9f8a-4fe3-bc98-d62f2328cb32");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "85040cd6-12ca-47f2-bb6b-811fb2b7779e");

            migrationBuilder.InsertData(
                table: "aspnetroles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6eda5005-b3a0-4890-9ba2-449fda36e05c", null, "Admin", "ADMIN" },
                    { "d355978f-dfe1-4bca-a17b-b19529afbd02", null, "User", "USER" }
                });
        }
    }
}
