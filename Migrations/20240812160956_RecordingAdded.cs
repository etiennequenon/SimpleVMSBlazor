using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleVMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class RecordingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Recording",
                table: "Cameras",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recording",
                table: "Cameras");
        }
    }
}
