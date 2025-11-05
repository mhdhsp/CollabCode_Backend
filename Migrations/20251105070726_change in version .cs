using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollabCode.Migrations
{
    /// <inheritdoc />
    public partial class changeinversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Projects_ProjectId",
                table: "Versions");

            migrationBuilder.DropIndex(
                name: "IX_Versions_ProjectId",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Versions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Versions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Versions_ProjectId",
                table: "Versions",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Projects_ProjectId",
                table: "Versions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
