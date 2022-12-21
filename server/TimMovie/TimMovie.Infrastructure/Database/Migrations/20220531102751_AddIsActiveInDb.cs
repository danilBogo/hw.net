using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Infrastructure.Database.Migrations
{
    public partial class AddIsActiveInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subscribes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscribes");
        }
    }
}
