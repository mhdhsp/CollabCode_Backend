using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollabCode.Migrations
{
    /// <inheritdoc />
    public partial class removedcurrentcodefromproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCode",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentCode",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
