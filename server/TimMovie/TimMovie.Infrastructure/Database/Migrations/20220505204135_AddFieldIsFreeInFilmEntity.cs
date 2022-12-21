using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimMovie.Infrastructure.Database.Migrations
{
    public partial class AddFieldIsFreeInFilmEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Films",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Films");
        }
    }
}
