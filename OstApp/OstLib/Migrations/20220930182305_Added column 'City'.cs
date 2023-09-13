using Microsoft.EntityFrameworkCore.Migrations;

namespace OstLib.Migrations
{
    public partial class AddedcolumnCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Client",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Client");
        }
    }
}
