using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SessionDesignFilePropAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesignFileId",
                table: "Sessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DesignFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_DesignFileId",
                table: "Sessions",
                column: "DesignFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_DesignFile_DesignFileId",
                table: "Sessions",
                column: "DesignFileId",
                principalTable: "DesignFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_DesignFile_DesignFileId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "DesignFile");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_DesignFileId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "DesignFileId",
                table: "Sessions");
        }
    }
}
