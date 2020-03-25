using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_earn_rule_contents_earn_rules_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_contents");

            migrationBuilder.DropTable(
                name: "earn_rules",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "spend_rule_contents",
                schema: "campaign");

            migrationBuilder.RenameColumn(
                name: "earn_rule_id",
                schema: "campaign",
                table: "earn_rule_contents",
                newName: "campaign_id");

            migrationBuilder.RenameIndex(
                name: "IX_earn_rule_contents_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_contents",
                newName: "IX_earn_rule_contents_campaign_id");

            migrationBuilder.CreateTable(
                name: "burn_rules",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    creation_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burn_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "burn_rule_contents",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    burn_rule_id = table.Column<Guid>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    localization = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burn_rule_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_burn_rule_contents_burn_rules_burn_rule_id",
                        column: x => x.burn_rule_id,
                        principalSchema: "campaign",
                        principalTable: "burn_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_burn_rule_contents_burn_rule_id",
                schema: "campaign",
                table: "burn_rule_contents",
                column: "burn_rule_id");

            migrationBuilder.AddForeignKey(
                name: "FK_earn_rule_contents_campaign_campaign_id",
                schema: "campaign",
                table: "earn_rule_contents",
                column: "campaign_id",
                principalSchema: "campaign",
                principalTable: "campaign",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_earn_rule_contents_campaign_campaign_id",
                schema: "campaign",
                table: "earn_rule_contents");

            migrationBuilder.DropTable(
                name: "burn_rule_contents",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "burn_rules",
                schema: "campaign");

            migrationBuilder.RenameColumn(
                name: "campaign_id",
                schema: "campaign",
                table: "earn_rule_contents",
                newName: "earn_rule_id");

            migrationBuilder.RenameIndex(
                name: "IX_earn_rule_contents_campaign_id",
                schema: "campaign",
                table: "earn_rule_contents",
                newName: "IX_earn_rule_contents_earn_rule_id");

            migrationBuilder.CreateTable(
                name: "earn_rules",
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
                    table.PrimaryKey("PK_earn_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "spend_rule_contents",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    campaign_id = table.Column<Guid>(nullable: false),
                    localization = table.Column<int>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spend_rule_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_spend_rule_contents_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalSchema: "campaign",
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_spend_rule_contents_campaign_id",
                schema: "campaign",
                table: "spend_rule_contents",
                column: "campaign_id");

            migrationBuilder.AddForeignKey(
                name: "FK_earn_rule_contents_earn_rules_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_contents",
                column: "earn_rule_id",
                principalSchema: "campaign",
                principalTable: "earn_rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
