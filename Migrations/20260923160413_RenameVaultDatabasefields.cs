using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class RenameVaultDatabasefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Website",
                table: "vault",
                newName: "EncryptedWebsite");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "vault",
                newName: "EncryptedNote");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "vault",
                newName: "EncryptedName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "vault",
                newName: "EncryptedEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EncryptedWebsite",
                table: "vault",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "EncryptedNote",
                table: "vault",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "EncryptedName",
                table: "vault",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EncryptedEmail",
                table: "vault",
                newName: "Email");
        }
    }
}
