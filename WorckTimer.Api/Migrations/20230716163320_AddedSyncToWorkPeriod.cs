using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorckTimer.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedSyncToWorkPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Synced",
                table: "WorkPeriods",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncedAt",
                table: "WorkPeriods",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Synced",
                table: "WorkPeriods");

            migrationBuilder.DropColumn(
                name: "SyncedAt",
                table: "WorkPeriods");
        }
    }
}
