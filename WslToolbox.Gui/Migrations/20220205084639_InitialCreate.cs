using Microsoft.EntityFrameworkCore.Migrations;

namespace WslToolbox.Gui.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributions",
                columns: table => new
                {
                    DistributionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DistributionGuid = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributions", x => x.DistributionId);
                });

            migrationBuilder.CreateTable(
                name: "QuickActions",
                columns: table => new
                {
                    QuickActionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuickActionCommand = table.Column<string>(type: "TEXT", nullable: true),
                    DistributionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickActions", x => x.QuickActionId);
                    table.ForeignKey(
                        name: "FK_QuickActions_Distributions_DistributionId",
                        column: x => x.DistributionId,
                        principalTable: "Distributions",
                        principalColumn: "DistributionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuickActions_DistributionId",
                table: "QuickActions",
                column: "DistributionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuickActions");

            migrationBuilder.DropTable(
                name: "Distributions");
        }
    }
}
