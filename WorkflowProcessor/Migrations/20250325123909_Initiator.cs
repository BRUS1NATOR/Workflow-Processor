using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class Initiator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "WorkflowInstances",
                newName: "parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowInstances_ParentId",
                table: "WorkflowInstances",
                newName: "IX_WorkflowInstances_parent_id");

            migrationBuilder.AddColumn<long>(
                name: "initiator",
                table: "WorkflowInstances",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "initiator",
                table: "WorkflowInstances");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "WorkflowInstances",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowInstances_parent_id",
                table: "WorkflowInstances",
                newName: "IX_WorkflowInstances_ParentId");
        }
    }
}
