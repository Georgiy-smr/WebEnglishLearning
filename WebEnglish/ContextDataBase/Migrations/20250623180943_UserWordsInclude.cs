using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContextDataBase.Migrations
{
    /// <inheritdoc />
    public partial class UserWordsInclude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Words",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_UserId",
                table: "Words",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Users_UserId",
                table: "Words",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Users_UserId",
                table: "Words");

            migrationBuilder.DropIndex(
                name: "IX_Words_UserId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Words");
        }
    }
}
