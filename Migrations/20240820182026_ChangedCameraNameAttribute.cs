using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleVMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCameraNameAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Cameras",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cameras",
                newName: "name");
        }
    }
}
