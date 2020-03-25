using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddPartnersAndVerticalToBurnRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "vertical",
                schema: "campaign",
                table: "burn_rules",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "burn_rule_partners",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    burn_rule_id = table.Column<Guid>(nullable: false),
                    partner_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burn_rule_partners", x => x.id);
                    table.ForeignKey(
                        name: "FK_burn_rule_partners_burn_rules_burn_rule_id",
                        column: x => x.burn_rule_id,
                        principalSchema: "campaign",
                        principalTable: "burn_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_burn_rule_partners_burn_rule_id",
                schema: "campaign",
                table: "burn_rule_partners",
                column: "burn_rule_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "burn_rule_partners",
                schema: "campaign");

            migrationBuilder.DropColumn(
                name: "vertical",
                schema: "campaign",
                table: "burn_rules");
        }
    }
}
