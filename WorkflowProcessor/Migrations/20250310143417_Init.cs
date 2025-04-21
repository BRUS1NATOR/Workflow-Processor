using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WorkflowProcessor.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    workflow_name = table.Column<string>(type: "text", nullable: false),
                    workflow_version = table.Column<int>(type: "integer", nullable: false),
                    data = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstances", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowExecutionPoints",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activity_id = table.Column<string>(type: "text", nullable: false),
                    activity_type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    workflow_instance_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowExecutionPoints", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkflowExecutionPoints_WorkflowInstances_workflow_instance~",
                        column: x => x.workflow_instance_id,
                        principalTable: "WorkflowInstances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowBookmarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    workflow_execution_point_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowBookmarks", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkflowBookmarks_WorkflowExecutionPoints_workflow_executio~",
                        column: x => x.workflow_execution_point_id,
                        principalTable: "WorkflowExecutionPoints",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowBookmarks_workflow_execution_point_id",
                table: "WorkflowBookmarks",
                column: "workflow_execution_point_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowExecutionPoints_workflow_instance_id",
                table: "WorkflowExecutionPoints",
                column: "workflow_instance_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowBookmarks");

            migrationBuilder.DropTable(
                name: "WorkflowExecutionPoints");

            migrationBuilder.DropTable(
                name: "WorkflowInstances");
        }
    }
}
