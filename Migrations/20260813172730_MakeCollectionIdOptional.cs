using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passwordmanagerbackend.Migrations
{
    /// <inheritdoc />
    public partial class MakeCollectionIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vault_collection_CollectionId",
                table: "vault");

            migrationBuilder.AlterColumn<int>(
                name: "CollectionId",
                table: "vault",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_vault_collection_CollectionId",
                table: "vault",
                column: "CollectionId",
                principalTable: "collection",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vault_collection_CollectionId",
                table: "vault");

            migrationBuilder.AlterColumn<int>(
                name: "CollectionId",
                table: "vault",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vault_collection_CollectionId",
                table: "vault",
                column: "CollectionId",
                principalTable: "collection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
