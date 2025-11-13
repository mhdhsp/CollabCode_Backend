using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollabCode.Migrations
{
    /// <inheritdoc />
    public partial class updatedcontent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "backup",
                table: "ProjectFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "backup",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
