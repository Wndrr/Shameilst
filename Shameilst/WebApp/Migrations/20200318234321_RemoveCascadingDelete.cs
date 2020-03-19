using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class RemoveCascadingDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Lists_ParentListId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Lists_TaskListEntityId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskListEntityId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskListEntityId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ParentListId",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Lists_ParentListId",
                table: "Tasks",
                column: "ParentListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Lists_ParentListId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ParentListId",
                table: "Tasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskListEntityId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskListEntityId",
                table: "Tasks",
                column: "TaskListEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Lists_ParentListId",
                table: "Tasks",
                column: "ParentListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Lists_TaskListEntityId",
                table: "Tasks",
                column: "TaskListEntityId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
