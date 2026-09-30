using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class StoreStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

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
                table: "tags");

            migrationBuilder.DropColumn(
                name: "image",
                table: "photos");

            migrationBuilder.DropColumn(
                name: "previewimage",
                table: "photos");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "aspnetusertokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "aspnetusers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "aspnetuserroles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "aspnetuserlogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "aspnetuserclaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "aspnetroles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "aspnetroleclaims");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "aspnetuserroles",
                newName: "IX_aspnetuserroles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "aspnetuserlogins",
                newName: "IX_aspnetuserlogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "aspnetuserclaims",
                newName: "IX_aspnetuserclaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "aspnetroleclaims",
                newName: "IX_aspnetroleclaims_RoleId");

            migrationBuilder.AddColumn<string>(
                name: "previewpath",
                table: "tags",
                type: "NVARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "shutterspeed",
                table: "photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exposure",
                table: "photos",
                type: "NVARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "caption",
                table: "photos",
                type: "NVARCHAR(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "photos",
                type: "NVARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "previewpath",
                table: "photos",
                type: "NVARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetusertokens",
                table: "aspnetusertokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetusers",
                table: "aspnetusers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetuserroles",
                table: "aspnetuserroles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetuserlogins",
                table: "aspnetuserlogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetuserclaims",
                table: "aspnetuserclaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetroles",
                table: "aspnetroles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aspnetroleclaims",
                table: "aspnetroleclaims",
                column: "Id");

            migrationBuilder.InsertData(
                table: "aspnetroles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6eda5005-b3a0-4890-9ba2-449fda36e05c", null, "Admin", "ADMIN" },
                    { "d355978f-dfe1-4bca-a17b-b19529afbd02", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetroleclaims_aspnetroles_RoleId",
                table: "aspnetroleclaims",
                column: "RoleId",
                principalTable: "aspnetroles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserclaims_aspnetusers_UserId",
                table: "aspnetuserclaims",
                column: "UserId",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserlogins_aspnetusers_UserId",
                table: "aspnetuserlogins",
                column: "UserId",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserroles_aspnetroles_RoleId",
                table: "aspnetuserroles",
                column: "RoleId",
                principalTable: "aspnetroles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserroles_aspnetusers_UserId",
                table: "aspnetuserroles",
                column: "UserId",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetusertokens_aspnetusers_UserId",
                table: "aspnetusertokens",
                column: "UserId",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aspnetroleclaims_aspnetroles_RoleId",
                table: "aspnetroleclaims");

            migrationBuilder.DropForeignKey(
                name: "FK_aspnetuserclaims_aspnetusers_UserId",
                table: "aspnetuserclaims");

            migrationBuilder.DropForeignKey(
                name: "FK_aspnetuserlogins_aspnetusers_UserId",
                table: "aspnetuserlogins");

            migrationBuilder.DropForeignKey(
                name: "FK_aspnetuserroles_aspnetroles_RoleId",
                table: "aspnetuserroles");

            migrationBuilder.DropForeignKey(
                name: "FK_aspnetuserroles_aspnetusers_UserId",
                table: "aspnetuserroles");

            migrationBuilder.DropForeignKey(
                name: "FK_aspnetusertokens_aspnetusers_UserId",
                table: "aspnetusertokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetusertokens",
                table: "aspnetusertokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetusers",
                table: "aspnetusers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetuserroles",
                table: "aspnetuserroles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetuserlogins",
                table: "aspnetuserlogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetuserclaims",
                table: "aspnetuserclaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetroles",
                table: "aspnetroles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aspnetroleclaims",
                table: "aspnetroleclaims");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "6eda5005-b3a0-4890-9ba2-449fda36e05c");

            migrationBuilder.DeleteData(
                table: "aspnetroles",
                keyColumn: "Id",
                keyValue: "d355978f-dfe1-4bca-a17b-b19529afbd02");

            migrationBuilder.DropColumn(
                name: "previewpath",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "path",
                table: "photos");

            migrationBuilder.DropColumn(
                name: "previewpath",
                table: "photos");

            migrationBuilder.RenameTable(
                name: "aspnetusertokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "aspnetusers",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "aspnetuserroles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "aspnetuserlogins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "aspnetuserclaims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "aspnetroles",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "aspnetroleclaims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_aspnetuserroles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_aspnetuserlogins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_aspnetuserclaims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_aspnetroleclaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddColumn<byte[]>(
                name: "previewimage",
                table: "tags",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "shutterspeed",
                table: "photos",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exposure",
                table: "photos",
                type: "NVARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "caption",
                table: "photos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "photos",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "previewimage",
                table: "photos",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ec18996-445b-443a-84f4-0691b967fe85", null, "Admin", "ADMIN" },
                    { "4e695943-aecc-454e-a7b3-1372bfe2effd", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
