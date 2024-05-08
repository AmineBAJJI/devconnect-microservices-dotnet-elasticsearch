using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Migrations
{
    /// <inheritdoc />
    public partial class update_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Profiles",
                newName: "Location");

            migrationBuilder.AddColumn<float>(
                name: "Rate",
                table: "Profiles",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Profiles",
                newName: "Address");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
