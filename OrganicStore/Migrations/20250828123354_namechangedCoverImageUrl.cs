using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganicStore.Migrations
{
    /// <inheritdoc />
    public partial class namechangedCoverImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "CoverImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Products",
                newName: "ImageUrl");
        }
    }
}
