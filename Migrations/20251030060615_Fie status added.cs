using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollabCode.Migrations
{
    /// <inheritdoc />
    public partial class Fiestatusadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectFiles");
        }
    }
}
