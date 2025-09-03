using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addedpreviewimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b6ff043-ab74-4aff-bf6a-d816390dd8e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be892897-3a61-4b92-bf4f-1e3c20e19353");

            migrationBuilder.AddColumn<byte[]>(
                name: "previewimage",
                table: "photos",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ec18996-445b-443a-84f4-0691b967fe85", null, "Admin", "ADMIN" },
                    { "4e695943-aecc-454e-a7b3-1372bfe2effd", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ec18996-445b-443a-84f4-0691b967fe85");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e695943-aecc-454e-a7b3-1372bfe2effd");

            migrationBuilder.DropColumn(
                name: "previewimage",
                table: "photos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b6ff043-ab74-4aff-bf6a-d816390dd8e7", null, "User", "USER" },
                    { "be892897-3a61-4b92-bf4f-1e3c20e19353", null, "Admin", "ADMIN" }
                });
        }
    }
}
