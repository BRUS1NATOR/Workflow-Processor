using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowExecutionPointId_renamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_WorkflowExecution~",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_WorkflowExecution~",
                table: "WorkflowBookmarks",
                column: "WorkflowExecutionPointId",
                principalTable: "WorkflowExecutionPoints",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
