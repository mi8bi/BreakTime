using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BreakTimeApp.Migrations.SelectNotifyDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectNotifyItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mode = table.Column<int>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectNotifyItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeStoreItemDb",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Span = table.Column<string>(type: "TEXT", nullable: false),
                    IsRunning = table.Column<bool>(type: "INTEGER", nullable: false),
                    Icon = table.Column<int>(type: "INTEGER", nullable: false),
                    Progress = table.Column<double>(type: "REAL", nullable: false),
                    MaxProgress = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeStoreItemDb", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectNotifyItems");

            migrationBuilder.DropTable(
                name: "TimeStoreItemDb");
        }
    }
}
