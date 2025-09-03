using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class cascadeBehaviorForPhotoTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_phototags_tags_Tagid",
                table: "phototags");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23d3e83c-51f6-4216-998f-6460a158aa66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2f44031-f883-46e3-a1b7-c8482477fb3c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9c2c94bc-d988-4e33-be53-165d0e26a576", null, "Admin", "ADMIN" },
                    { "cc331e37-911f-49fe-92e0-9557ae58cae8", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_phototags_tags_Tagid",
                table: "phototags",
                column: "Tagid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_phototags_tags_Tagid",
                table: "phototags");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c2c94bc-d988-4e33-be53-165d0e26a576");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc331e37-911f-49fe-92e0-9557ae58cae8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "23d3e83c-51f6-4216-998f-6460a158aa66", null, "User", "USER" },
                    { "e2f44031-f883-46e3-a1b7-c8482477fb3c", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_phototags_tags_Tagid",
                table: "phototags",
                column: "Tagid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
