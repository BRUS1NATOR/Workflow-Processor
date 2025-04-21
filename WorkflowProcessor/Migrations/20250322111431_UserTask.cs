using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class UserTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkflowBookmarks");

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    workflow_bookmark_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => new { x.user_id, x.workflow_bookmark_id });
                    table.ForeignKey(
                        name: "FK_UserTasks_WorkflowBookmarks_workflow_bookmark_id",
                        column: x => x.workflow_bookmark_id,
                        principalTable: "WorkflowBookmarks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_workflow_bookmark_id",
                table: "UserTasks",
                column: "workflow_bookmark_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WorkflowBookmarks",
                type: "integer",
                nullable: true);
        }
    }
}
