using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Migrations
{
    public partial class changecolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AppUsers_TragetUserId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "TragetUserId",
                table: "Likes",
                newName: "TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_TragetUserId",
                table: "Likes",
                newName: "IX_Likes_TargetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AppUsers_TargetUserId",
                table: "Likes",
                column: "TargetUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AppUsers_TargetUserId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "TargetUserId",
                table: "Likes",
                newName: "TragetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_TargetUserId",
                table: "Likes",
                newName: "IX_Likes_TragetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AppUsers_TragetUserId",
                table: "Likes",
                column: "TragetUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
