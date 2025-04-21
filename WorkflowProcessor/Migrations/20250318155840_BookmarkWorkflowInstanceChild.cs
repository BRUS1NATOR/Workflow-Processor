using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class BookmarkWorkflowInstanceChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_workflow_executio~",
                table: "WorkflowBookmarks");

            migrationBuilder.RenameColumn(
                name: "workflow_execution_point_id",
                table: "WorkflowBookmarks",
                newName: "WorkflowExecutionPointId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowBookmarks_workflow_execution_point_id",
                table: "WorkflowBookmarks",
                newName: "IX_WorkflowBookmarks_WorkflowExecutionPointId");

            migrationBuilder.AddColumn<int>(
                name: "workflow_child_id",
                table: "WorkflowBookmarks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowBookmarks_workflow_child_id",
                table: "WorkflowBookmarks",
                column: "workflow_child_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_WorkflowExecution~",
                table: "WorkflowBookmarks",
                column: "WorkflowExecutionPointId",
                principalTable: "WorkflowExecutionPoints",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowInstances_workflow_child_id",
                table: "WorkflowBookmarks",
                column: "workflow_child_id",
                principalTable: "WorkflowInstances",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_WorkflowExecution~",
                table: "WorkflowBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowInstances_workflow_child_id",
                table: "WorkflowBookmarks");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowBookmarks_workflow_child_id",
                table: "WorkflowBookmarks");

            migrationBuilder.DropColumn(
                name: "workflow_child_id",
                table: "WorkflowBookmarks");

            migrationBuilder.RenameColumn(
                name: "WorkflowExecutionPointId",
                table: "WorkflowBookmarks",
                newName: "workflow_execution_point_id");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowBookmarks_WorkflowExecutionPointId",
                table: "WorkflowBookmarks",
                newName: "IX_WorkflowBookmarks_workflow_execution_point_id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_workflow_executio~",
                table: "WorkflowBookmarks",
                column: "workflow_execution_point_id",
                principalTable: "WorkflowExecutionPoints",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
