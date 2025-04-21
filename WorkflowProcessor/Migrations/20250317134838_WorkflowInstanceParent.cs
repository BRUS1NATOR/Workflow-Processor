using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowInstanceParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "WorkflowInstances",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstances_ParentId",
                table: "WorkflowInstances",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "parent_children_fk",
                table: "WorkflowInstances",
                column: "ParentId",
                principalTable: "WorkflowInstances",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "parent_children_fk",
                table: "WorkflowInstances");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowInstances_ParentId",
                table: "WorkflowInstances");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "WorkflowInstances");
        }
    }
}
