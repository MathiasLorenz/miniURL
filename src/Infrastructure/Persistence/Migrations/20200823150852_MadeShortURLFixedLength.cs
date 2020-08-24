using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniURL.Infrastructure.Persistence.Migrations
{
    public partial class MadeShortURLFixedLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortURL",
                table: "PersistedURLs",
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortURL",
                table: "PersistedURLs",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 8);
        }
    }
}
