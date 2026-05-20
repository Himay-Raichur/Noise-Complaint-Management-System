using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiseComplaint.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComplaintModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoiseType",
                table: "Complaints",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Complaints",
                newName: "NoiseType");
        }
    }
}
