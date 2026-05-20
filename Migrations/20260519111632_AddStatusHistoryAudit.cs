using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiseComplaint.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusHistoryAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "StatusHistories");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "StatusHistories");

            migrationBuilder.RenameColumn(
                name: "ToStatus",
                table: "StatusHistories",
                newName: "OldStatus");

            migrationBuilder.RenameColumn(
                name: "FromStatus",
                table: "StatusHistories",
                newName: "NewStatus");

            migrationBuilder.AddColumn<int>(
                name: "ChangedByUserId",
                table: "StatusHistories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_ChangedByUserId",
                table: "StatusHistories",
                column: "ChangedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusHistories_Users_ChangedByUserId",
                table: "StatusHistories",
                column: "ChangedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusHistories_Users_ChangedByUserId",
                table: "StatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_StatusHistories_ChangedByUserId",
                table: "StatusHistories");

            migrationBuilder.DropColumn(
                name: "ChangedByUserId",
                table: "StatusHistories");

            migrationBuilder.RenameColumn(
                name: "OldStatus",
                table: "StatusHistories",
                newName: "ToStatus");

            migrationBuilder.RenameColumn(
                name: "NewStatus",
                table: "StatusHistories",
                newName: "FromStatus");

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "StatusHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "StatusHistories",
                type: "TEXT",
                nullable: true);
        }
    }
}
