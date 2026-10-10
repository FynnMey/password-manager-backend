using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePremiumField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "users");

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumUntil",
                table: "users",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumUntil",
                table: "users");

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
