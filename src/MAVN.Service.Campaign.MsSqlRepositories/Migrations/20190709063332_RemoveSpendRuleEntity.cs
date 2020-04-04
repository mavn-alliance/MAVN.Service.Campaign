using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class RemoveSpendRuleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spend_rule_contents_spend_rules_spend_rule_id",
                schema: "campaign",
                table: "spend_rule_contents");

            migrationBuilder.DropTable(
                name: "spend_rules",
                schema: "campaign");

            migrationBuilder.RenameColumn(
                name: "spend_rule_id",
                schema: "campaign",
                table: "spend_rule_contents",
                newName: "campaign_id");

            migrationBuilder.RenameIndex(
                name: "IX_spend_rule_contents_spend_rule_id",
                schema: "campaign",
                table: "spend_rule_contents",
                newName: "IX_spend_rule_contents_campaign_id");

            migrationBuilder.AddForeignKey(
                name: "FK_spend_rule_contents_campaign_campaign_id",
                schema: "campaign",
                table: "spend_rule_contents",
                column: "campaign_id",
                principalSchema: "campaign",
                principalTable: "campaign",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_spend_rule_contents_campaign_campaign_id",
                schema: "campaign",
                table: "spend_rule_contents");

            migrationBuilder.RenameColumn(
                name: "campaign_id",
                schema: "campaign",
                table: "spend_rule_contents",
                newName: "spend_rule_id");

            migrationBuilder.RenameIndex(
                name: "IX_spend_rule_contents_campaign_id",
                schema: "campaign",
                table: "spend_rule_contents",
                newName: "IX_spend_rule_contents_spend_rule_id");

            migrationBuilder.CreateTable(
                name: "spend_rules",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    creation_date = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spend_rules", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_spend_rule_contents_spend_rules_spend_rule_id",
                schema: "campaign",
                table: "spend_rule_contents",
                column: "spend_rule_id",
                principalSchema: "campaign",
                principalTable: "spend_rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
