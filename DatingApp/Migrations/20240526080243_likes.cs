using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TragetUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.SourceUserId, x.TragetUserId });
                    table.ForeignKey(
                        name: "FK_Likes_AppUsers_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_AppUsers_TragetUserId",
                        column: x => x.TragetUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_TragetUserId",
                table: "Likes",
                column: "TragetUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
