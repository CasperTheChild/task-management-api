using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.Services.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoListUsersRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_AspNetUsers_UserId",
                table: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_UserId",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoLists");

            migrationBuilder.CreateTable(
                name: "TodoListUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TodoListId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoListUsers", x => new { x.TodoListId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TodoListUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoListUsers_TodoLists_TodoListId",
                        column: x => x.TodoListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoListUsers_UserId",
                table: "TodoListUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoListUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TodoLists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_UserId",
                table: "TodoLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_AspNetUsers_UserId",
                table: "TodoLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
