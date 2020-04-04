using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddOrderToBonusTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                schema: "campaign",
                table: "bonus_type",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                schema: "campaign",
                table: "bonus_type");
        }
    }
}
