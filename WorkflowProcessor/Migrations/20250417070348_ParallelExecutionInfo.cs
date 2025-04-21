using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class ParallelExecutionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "activity_id",
                table: "WorkflowExecutionPoints",
                newName: "step_id");

            migrationBuilder.CreateTable(
                name: "ParallelExecutionInfo",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    workflow_instance_id = table.Column<long>(type: "bigint", nullable: false),
                    activity_id = table.Column<string>(type: "text", nullable: false),
                    activated_connections_count = table.Column<int>(type: "integer", nullable: false),
                    finished_connections_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParallelExecutionInfo", x => x.id);
                    table.ForeignKey(
                        name: "parent_children_fk",
                        column: x => x.workflow_instance_id,
                        principalTable: "WorkflowInstances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParallelExecutionInfo_workflow_instance_id",
                table: "ParallelExecutionInfo",
                column: "workflow_instance_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParallelExecutionInfo");

            migrationBuilder.RenameColumn(
                name: "step_id",
                table: "WorkflowExecutionPoints",
                newName: "activity_id");
        }
    }
}
