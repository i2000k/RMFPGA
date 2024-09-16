using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConnectivityOfStandsAndUsersWithSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Stands_StandId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_User_UserId",
                table: "Sessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Sessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StandId",
                table: "Sessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Stands_StandId",
                table: "Sessions",
                column: "StandId",
                principalTable: "Stands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_User_UserId",
                table: "Sessions",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Stands_StandId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_User_UserId",
                table: "Sessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Sessions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "StandId",
                table: "Sessions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Stands_StandId",
                table: "Sessions",
                column: "StandId",
                principalTable: "Stands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_User_UserId",
                table: "Sessions",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
