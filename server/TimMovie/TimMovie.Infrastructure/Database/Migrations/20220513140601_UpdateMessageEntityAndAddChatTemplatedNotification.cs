using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Infrastructure.Database.Migrations
{
    public partial class UpdateMessageEntityAndAddChatTemplatedNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SupportId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SupportId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SupportId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChatTemplatedNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatTemplatedNotifications", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatTemplatedNotifications");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "SupportId",
                table: "Messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SupportId",
                table: "Messages",
                column: "SupportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SupportId",
                table: "Messages",
                column: "SupportId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
