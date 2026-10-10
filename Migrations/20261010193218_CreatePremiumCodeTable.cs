using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class CreatePremiumCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "premium_code",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RedeemedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RedeemedByUserId = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium_code", x => x.Code);
                    table.ForeignKey(
                        name: "FK_premium_code_users_RedeemedByUserId",
                        column: x => x.RedeemedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_premium_code_RedeemedByUserId",
                table: "premium_code",
                column: "RedeemedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "premium_code");
        }
    }
}
