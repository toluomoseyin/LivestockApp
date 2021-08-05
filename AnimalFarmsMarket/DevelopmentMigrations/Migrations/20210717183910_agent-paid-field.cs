using Microsoft.EntityFrameworkCore.Migrations;

namespace DevelopmentMigrations.Migrations
{
    public partial class agentpaidfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "AgentPaid",
                table: "OrderItems",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                table: "Agents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentPaid",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Recipient",
                table: "Agents");
        }
    }
}
