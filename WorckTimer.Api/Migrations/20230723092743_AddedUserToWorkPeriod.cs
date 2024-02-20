using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorckTimer.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserToWorkPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Synced",
                table: "WorkPeriods");

            migrationBuilder.DropColumn(
                name: "SyncedAt",
                table: "WorkPeriods");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndAt",
                table: "WorkPeriods",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WorkPeriods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[] { 1, "admin", "admin", "admin", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPeriods_UserId",
                table: "WorkPeriods",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkPeriods_Users_UserId",
                table: "WorkPeriods",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkPeriods_Users_UserId",
                table: "WorkPeriods");

            migrationBuilder.DropIndex(
                name: "IX_WorkPeriods_UserId",
                table: "WorkPeriods");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkPeriods");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndAt",
                table: "WorkPeriods",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

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
    }
}
