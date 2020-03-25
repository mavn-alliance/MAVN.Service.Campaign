using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "earn_rules",
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
                    table.PrimaryKey("PK_earn_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "spend_rules",
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
                    table.PrimaryKey("PK_spend_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "earn_rule_content",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    earn_rule_id = table.Column<Guid>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    localization = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_earn_rule_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_earn_rule_content_earn_rules_earn_rule_id",
                        column: x => x.earn_rule_id,
                        principalSchema: "campaign",
                        principalTable: "earn_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "spend_rule_contents",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    spend_rule_id = table.Column<Guid>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    localization = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spend_rule_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_spend_rule_contents_spend_rules_spend_rule_id",
                        column: x => x.spend_rule_id,
                        principalSchema: "campaign",
                        principalTable: "spend_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_earn_rule_content_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_content",
                column: "earn_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_spend_rule_contents_spend_rule_id",
                schema: "campaign",
                table: "spend_rule_contents",
                column: "spend_rule_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "earn_rule_content",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "spend_rule_contents",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "earn_rules",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "spend_rules",
                schema: "campaign");
        }
    }
}
