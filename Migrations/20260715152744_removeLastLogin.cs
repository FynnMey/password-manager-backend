using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class removeLastLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "users",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
