using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class addSaltToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "users",
                type: "varchar(44)",
                maxLength: 44,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "users");
        }
    }
}
