using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class AddTaskListsAndTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskListEntityId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lists_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ParentListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Lists_ParentListId",
                        column: x => x.ParentListId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TaskListEntityId",
                table: "AspNetUsers",
                column: "TaskListEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_OwnerId",
                table: "Lists",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentListId",
                table: "Tasks",
                column: "ParentListId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lists_TaskListEntityId",
                table: "AspNetUsers",
                column: "TaskListEntityId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lists_TaskListEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TaskListEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaskListEntityId",
                table: "AspNetUsers");
        }
    }
}
