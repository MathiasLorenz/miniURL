using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniURL.Infrastructure.Persistence.Migrations
{
    public partial class AddedPersistedUrlEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersistedURLs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    URL = table.Column<string>(maxLength: 300, nullable: false),
                    ShortURL = table.Column<string>(maxLength: 8, nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedURLs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersistedURLs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedURLs_UserId",
                table: "PersistedURLs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedURLs");
        }
    }
}
