using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NationalPark_API.Migrations
{
    /// <inheritdoc />
    public partial class jdkfjasldk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trails_NationalParks_NationalParkedId",
                table: "Trails");

            migrationBuilder.DropIndex(
                name: "IX_Trails_NationalParkedId",
                table: "Trails");

            migrationBuilder.DropColumn(
                name: "NationalParkedId",
                table: "Trails");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails",
                column: "NationalParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trails_NationalParks_NationalParkId",
                table: "Trails",
                column: "NationalParkId",
                principalTable: "NationalParks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trails_NationalParks_NationalParkId",
                table: "Trails");

            migrationBuilder.DropIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NationalParkedId",
                table: "Trails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trails_NationalParkedId",
                table: "Trails",
                column: "NationalParkedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trails_NationalParks_NationalParkedId",
                table: "Trails",
                column: "NationalParkedId",
                principalTable: "NationalParks",
                principalColumn: "Id");
        }
    }
}
