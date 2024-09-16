using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SessionDesignFileTypeChangedToByteArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_DesignFile_DesignFileId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_DesignFileId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignFile",
                table: "DesignFile");

            migrationBuilder.DropColumn(
                name: "DesignFileId",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "DesignFile",
                newName: "DesignFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "DesignFile",
                table: "Sessions",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignFiles",
                table: "DesignFiles",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignFiles",
                table: "DesignFiles");

            migrationBuilder.DropColumn(
                name: "DesignFile",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "DesignFiles",
                newName: "DesignFile");

            migrationBuilder.AddColumn<Guid>(
                name: "DesignFileId",
                table: "Sessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignFile",
                table: "DesignFile",
                column: "Id");

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
    }
}
