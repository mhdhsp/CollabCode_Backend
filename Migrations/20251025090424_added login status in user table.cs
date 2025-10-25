using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollabCode.Migrations
{
    /// <inheritdoc />
    public partial class addedloginstatusinusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Users");
        }
    }
}
