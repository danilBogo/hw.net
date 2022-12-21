using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Infrastructure.Database.Migrations
{
    public partial class RenameUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "UserStatus");

            migrationBuilder.AddColumn<int>(
                name: "StatusEnum",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusEnum",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DateLastChange = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserStatusEnum = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatus_AspNetUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStatus_UserGuid",
                table: "UserStatus",
                column: "UserGuid",
                unique: true);
        }
    }
}
