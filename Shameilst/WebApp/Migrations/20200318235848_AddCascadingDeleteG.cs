using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class AddCascadingDeleteG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskListEntityId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskListEntityId",
                table: "Tasks",
                column: "TaskListEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Lists_TaskListEntityId",
                table: "Tasks",
                column: "TaskListEntityId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Lists_TaskListEntityId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskListEntityId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskListEntityId",
                table: "Tasks");
        }
    }
}
