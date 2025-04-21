using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class ActivitiesToFinishIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParallelExecutionInfo");

            migrationBuilder.AddColumn<string[]>(
                name: "activated_steps_ids",
                table: "WorkflowExecutionPoints",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activated_steps_ids",
                table: "WorkflowExecutionPoints");

            migrationBuilder.CreateTable(
                name: "ParallelExecutionInfo",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    workflow_instance_id = table.Column<long>(type: "bigint", nullable: false),
                    activated_connections_count = table.Column<int>(type: "integer", nullable: false),
                    activity_id = table.Column<string>(type: "text", nullable: false),
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
    }
}
