using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganicStore.Migrations
{
    /// <inheritdoc />
    public partial class changename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaderInfos",
                table: "HeaderInfos");

            migrationBuilder.RenameTable(
                name: "HeaderInfos",
                newName: "ContactInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos");

            migrationBuilder.RenameTable(
                name: "ContactInfos",
                newName: "HeaderInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaderInfos",
                table: "HeaderInfos",
                column: "Id");
        }
    }
}
