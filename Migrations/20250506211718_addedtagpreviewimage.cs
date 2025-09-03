using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addedtagpreviewimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c2c94bc-d988-4e33-be53-165d0e26a576");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc331e37-911f-49fe-92e0-9557ae58cae8");

            migrationBuilder.AlterColumn<string>(
                name: "tag",
                table: "tags",
                type: "NVARCHAR(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "previewimage",
                table: "tags",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exposure",
                table: "photos",
                type: "NVARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "photos",
                type: "NVARCHAR(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1000)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b6ff043-ab74-4aff-bf6a-d816390dd8e7", null, "User", "USER" },
                    { "be892897-3a61-4b92-bf4f-1e3c20e19353", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b6ff043-ab74-4aff-bf6a-d816390dd8e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be892897-3a61-4b92-bf4f-1e3c20e19353");

            migrationBuilder.DropColumn(
                name: "previewimage",
                table: "tags");

            migrationBuilder.AlterColumn<string>(
                name: "tag",
                table: "tags",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "exposure",
                table: "photos",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(10)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "photos",
                type: "VARCHAR(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(1000)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9c2c94bc-d988-4e33-be53-165d0e26a576", null, "Admin", "ADMIN" },
                    { "cc331e37-911f-49fe-92e0-9557ae58cae8", null, "User", "USER" }
                });
        }
    }
}
