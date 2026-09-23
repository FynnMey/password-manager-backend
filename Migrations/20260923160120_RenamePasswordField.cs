using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class RenamePasswordField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "vault",
                newName: "EncryptedPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EncryptedPassword",
                table: "vault",
                newName: "Password");
        }
    }
}
