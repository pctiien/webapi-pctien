using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class reff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayDat",
                table: "DonHang",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 18, 15, 25, 52, 515, DateTimeKind.Utc).AddTicks(7292),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 7, 15, 8, 17, 0, 180, DateTimeKind.Utc).AddTicks(8741));

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: false),
                    JwtId = table.Column<string>(type: "longtext", nullable: false),
                    IsUsed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayDat",
                table: "DonHang",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 15, 8, 17, 0, 180, DateTimeKind.Utc).AddTicks(8741),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 7, 18, 15, 25, 52, 515, DateTimeKind.Utc).AddTicks(7292));
        }
    }
}
