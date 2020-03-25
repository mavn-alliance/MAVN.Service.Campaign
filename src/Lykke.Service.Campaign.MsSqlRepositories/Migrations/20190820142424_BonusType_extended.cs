using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class BonusType_extended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_infinite",
                schema: "campaign",
                table: "bonus_type",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "allow_percentage",
                schema: "campaign",
                table: "bonus_type",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "vertical",
                schema: "campaign",
                table: "bonus_type",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_infinite",
                schema: "campaign",
                table: "bonus_type");

            migrationBuilder.DropColumn(
                name: "allow_percentage",
                schema: "campaign",
                table: "bonus_type");

            migrationBuilder.DropColumn(
                name: "vertical",
                schema: "campaign",
                table: "bonus_type");
        }
    }
}
