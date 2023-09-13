using Microsoft.EntityFrameworkCore.Migrations;

namespace OstLib.Migrations
{
    public partial class AddedfieldsGinekologiaInjuryandOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_PhoneNumber",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Appointment",
                newName: "Heal");

            migrationBuilder.AddColumn<string>(
                name: "Ginekologia",
                table: "Client",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Injury",
                table: "Client",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "Client",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complaint",
                table: "Appointment",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ginekologia",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Injury",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Operation",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Complaint",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "Heal",
                table: "Appointment",
                newName: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PhoneNumber",
                table: "Client",
                column: "PhoneNumber",
                unique: true);
        }
    }
}
