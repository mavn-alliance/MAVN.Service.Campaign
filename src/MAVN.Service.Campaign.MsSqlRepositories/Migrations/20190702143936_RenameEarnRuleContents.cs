using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class RenameEarnRuleContents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_earn_rule_content_earn_rules_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_content");

            migrationBuilder.DropPrimaryKey(
                name: "PK_earn_rule_content",
                schema: "campaign",
                table: "earn_rule_content");

            migrationBuilder.RenameTable(
                name: "earn_rule_content",
                schema: "campaign",
                newName: "earn_rule_contents",
                newSchema: "campaign");

            migrationBuilder.RenameIndex(
                name: "IX_earn_rule_content_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_contents",
                newName: "IX_earn_rule_contents_earn_rule_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_earn_rule_contents",
                schema: "campaign",
                table: "earn_rule_contents",
                column: "id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_earn_rule_contents_earn_rules_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_contents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_earn_rule_contents",
                schema: "campaign",
                table: "earn_rule_contents");

            migrationBuilder.RenameTable(
                name: "earn_rule_contents",
                schema: "campaign",
                newName: "earn_rule_content",
                newSchema: "campaign");

            migrationBuilder.RenameIndex(
                name: "IX_earn_rule_contents_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_content",
                newName: "IX_earn_rule_content_earn_rule_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_earn_rule_content",
                schema: "campaign",
                table: "earn_rule_content",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_earn_rule_content_earn_rules_earn_rule_id",
                schema: "campaign",
                table: "earn_rule_content",
                column: "earn_rule_id",
                principalSchema: "campaign",
                principalTable: "earn_rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
